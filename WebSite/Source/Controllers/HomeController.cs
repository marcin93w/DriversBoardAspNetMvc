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

//<div>Icons made by<a href="http://www.freepik.com" title="Freepik"> Freepik</a> from<a href="http://www.flaticon.com" title="Flaticon"> www.flaticon.com</a> is licensed by <a href = "http://creativecommons.org/licenses/by/3.0/" title= "Creative Commons BY 3.0" target= "_blank" > CC 3.0 BY</a></div>
namespace Driver.WebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IDriversRepository _driversRepository;

        public HomeController(IItemsRepository itemsRepository, IDriversRepository driversRepository)
        {
            _itemsRepository = itemsRepository;
            _driversRepository = driversRepository;
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

            var item = Mapper.Map<AddItemViewModel, Item>(addItemViewModel);
            item.Author = new ApplicationUser { Id = this.GetCurrentUserId() };
            item.DateAdded = DateTime.Now;

            var driverOccurrencesAddViewModels = addItemViewModel.Drivers
                .Where(viewModel => !string.IsNullOrEmpty(viewModel.Plate));
            var driverOccurences = new List<DriverOccurrence>();
            foreach (var driverOccurrenceVm in driverOccurrencesAddViewModels)
            {
                driverOccurences.Add(await CreateDriverOccurrenceFromAddDriverViewModel(driverOccurrenceVm));
            }
            item.DriversOccurrences = driverOccurences;

            await _itemsRepository.AddItem(item);

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
            viewModel.DriverOccurrences = item.DriversOccurrences.Select(CreateDriverOccurrenceViewModel);
            return viewModel;
        }

        private DriverOccurrenceViewModel CreateDriverOccurrenceViewModel(DriverOccurrence driverOccurrence)
        {
            var viewModel = new DriverOccurrenceViewModel
            {
                Id = driverOccurrence.Id,
                DriverId = driverOccurrence.Driver.Id,
                Plate = FormatPlateDisplay(driverOccurrence.Driver.Plate),
                Description = driverOccurrence.Description ?? driverOccurrence.Driver.Description,
                DownVotesCount = driverOccurrence.DownVotesCount,
                StartSecond = driverOccurrence.StartSecond,
                EndSecond = driverOccurrence.EndSecond
            };

            bool? userVotedPositive = driverOccurrence.Votes.FirstOrDefault()?.Positive;
            viewModel.UserVote = userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;

            return viewModel;
        }

        private string FormatPlateDisplay(string plate)
        {
            plate = plate.Replace(" ", string.Empty);
            var indexToInsertSpace = char.IsLetter(plate[2]) ? 3 : 2;
            return plate.Insert(indexToInsertSpace, " ");
        }

        public async Task<DriverOccurrence> CreateDriverOccurrenceFromAddDriverViewModel(AddDriverOccurrenceViewModel viewModel)
        {
            var driver = await _driversRepository.FindDriverByPlate(viewModel.Plate) ?? 
                new Models.Driver
                {
                    Plate = viewModel.Plate,
                    Description = viewModel.Description
                };

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
            var item = await _itemsRepository.GetItem(addCommentViewModel.ItemId, this.GetCurrentUserId());

            var comment = new Comment
            {
                Item = item,
                Text = addCommentViewModel.Text,
                User = new ApplicationUser { Id = this.GetCurrentUserId() },
                DateTime = DateTime.Now
            };

            try
            {
                await _itemsRepository.AddComment(comment);
            }
            catch (Exception ex)
            {
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