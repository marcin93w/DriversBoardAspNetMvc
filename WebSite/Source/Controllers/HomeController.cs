using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Driver.WebSite.DAL;
using Driver.WebSite.Models;
using Driver.WebSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ApplicationDbContext = Driver.WebSite.DAL.ApplicationDbContext;
using Driver = Driver.WebSite.Models.Driver;

//Signals Set graphic by <a href="http://www.freepik.com/">Freepik</a> from <a href="http://www.flaticon.com/">Flaticon</a> is licensed under <a href="http://creativecommons.org/licenses/by/3.0/" title="Creative Commons BY 3.0">CC BY 3.0</a>. Made with <a href="http://logomakr.com" title="Logo Maker">Logo Maker</a>

namespace Driver.WebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; //TODO should be deleted
        private readonly IItemsRepository _itemsRepository;

        public HomeController(ApplicationDbContext context, IItemsRepository itemsRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
            context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(new HomeViewModel(await GetAllItemsFromRepositoryAndConvertThemToViewModels()));
        }

        public ActionResult AddItem()
        {
            return View(new AddItemViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> SaveItem(AddItemViewModel addItemViewModel)
        {
            if (addItemViewModel.ContentType == ItemContentType.NotSupported)
            {
                return View("AddItem", addItemViewModel);
            }

            var store = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(store);
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var item = Mapper.Map<AddItemViewModel, Item>(addItemViewModel);
            item.Author = user;
            item.DateAdded = DateTime.Now;
            item.DriversOccurrences = addItemViewModel.Drivers
                .Where(viewModel => !string.IsNullOrEmpty(viewModel.Plate))
                .Select(CreateDriverOccurrenceFromAddDriverViewModel).ToArray();

            _context.Items.Add(item);

            await _context.SaveChangesAsync();

            return View("Index", new HomeViewModel(await GetAllItemsFromRepositoryAndConvertThemToViewModels()) { DisplayAddedInfo = true });
        }

        private async Task<IEnumerable<ItemPanelViewModel>> GetAllItemsFromRepositoryAndConvertThemToViewModels()
        {
            return CreateItemPanelViewModels(await _itemsRepository.GetAllItems(this.GetCurrentUserId()));
        }

        private IEnumerable<ItemPanelViewModel> CreateItemPanelViewModels(IEnumerable<Item> items)
        {
            return items.Select(CreateItemPanelViewModel);
        }

        private ItemPanelViewModel CreateItemPanelViewModel(Item item)
        {
            var viewModel = Mapper.Map<Item, ItemPanelViewModel>(item);
            viewModel.AuthorLogin = item.Author.UserName;
            bool? userVotedPositive = item.Votes.FirstOrDefault()?.Positive;
            viewModel.UserVoting = userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;
            viewModel.Drivers = item.DriversOccurrences.Select(CreateDriverOccurrenceViewModel);
            return viewModel;
        }

        private DriverOccurrenceViewModel CreateDriverOccurrenceViewModel(DriverOccurrence driverOccurrence)
        {
            return new DriverOccurrenceViewModel
            {
                Id = driverOccurrence.Driver.Id,
                Plate = driverOccurrence.Driver.Plate,
                Description = driverOccurrence.Description ?? driverOccurrence.Driver.Description,
                DownVotesCount = driverOccurrence.DownVotesCount,
                StartSecond = driverOccurrence.StartSecond,
                EndSecond = driverOccurrence.EndSecond
            };
        }

        public DriverOccurrence CreateDriverOccurrenceFromAddDriverViewModel(AddDriverOccurrenceViewModel viewModel)
        {
            var driver = new Models.Driver
            {
                Plate = viewModel.Plate
            };

            if (viewModel.DriverId.HasValue)
            {
                driver.Id = viewModel.DriverId.Value;
                _context.Set<Models.Driver>().Attach(driver);
            }
            else
            {
                driver.Description = viewModel.Description;
            }

            return new DriverOccurrence
            {
                Driver = driver,
                Description = viewModel.Description,
            };
        }

        [AllowAnonymous]
        public async Task<ActionResult> ItemPage(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var item = (await _itemsRepository.GetItem(id.Value, this.GetCurrentUserId()));

            if (item != null)
                return View(new ItemPageViewModel(CreateItemPanelViewModel(item), item.Comments.Select(CreateCommentViewModel)));
            else
                return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(AddCommentViewModel addCommentViewModel)
        {
            var user = await this.GetCurrentUserAsync(_context);
            var item = await _itemsRepository.GetItem(addCommentViewModel.ItemId, user.Id);

            var comment = new Comment
            {
                Item = item,
                Text = addCommentViewModel.Text,
                User = user,
                DateTime = DateTime.Now
            };

            _context.Comments.Add(comment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.Comments.Remove(comment);
                return View("ItemPage", new ItemPageViewModel(CreateItemPanelViewModel(item), item.Comments.Select(CreateCommentViewModel), ex));
            }

            return View("ItemPage", new ItemPageViewModel(CreateItemPanelViewModel( item ), item.Comments.Select(CreateCommentViewModel), comment.Id));
        }

        private CommentViewModel CreateCommentViewModel(Comment comment)
        {
            var viewModel = Mapper.Map<Comment, CommentViewModel>(comment);
            viewModel.AuthorLogin = comment.User.UserName;
            bool? userVotedPositive = comment.CommentVotes.FirstOrDefault()?.Positive;
            viewModel.UserVote = userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;
            viewModel.VotesCount = comment.GetVotesCount();
            return viewModel;
        }
    }
}