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

namespace Driver.WebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private const int SidebarDriversRankingCount = 5;
        private const int PageItemsCount = 5;

        private readonly IItemsRepository _itemsRepository;
        private readonly IDriversRepository _driversRepository;

        public HomeController(IItemsRepository itemsRepository, IDriversRepository driversRepository)
        {
            _itemsRepository = itemsRepository;
            _driversRepository = driversRepository;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page)
        {
            return View(await PrepareHomeViewModel());
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

            var homeViewModel = await PrepareHomeViewModel();
            homeViewModel.DisplayAddedInfo = true;

            return View("Index", homeViewModel);
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
            var viewModel = Mapper.Map<DriverOccurrenceViewModel>(driverOccurrence);
            if (string.IsNullOrEmpty(viewModel.Description))
            {
                viewModel.Description = driverOccurrence.Driver.Description;
            }

            bool? userVotedPositive = driverOccurrence.Votes.FirstOrDefault()?.Positive;
            viewModel.UserVote = userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;

            return viewModel;
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
                return View(await PrepareItemPageViewModel(item));
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
                return View("ItemPage", await PrepareItemPageViewModel(item, null, ex));
            }

            return View("ItemPage", await PrepareItemPageViewModel(item, comment.Id));
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

        private async Task<HomeViewModel> PrepareHomeViewModel()
        {
            var items = await _itemsRepository.GetAllItems(this.GetCurrentUserId());
            var itemPanelViewModels = CreateItemPanelViewModels(items);
            var sidebar = await PrepareSidebar();
            return new HomeViewModel
            {
                Items = itemPanelViewModels,
                Sidebar = sidebar
            };
        }

        private async Task<ItemPageViewModel> PrepareItemPageViewModel(Item item, 
            int? addedCommentId = null, Exception commentAddingException = null)
        {
            ItemPageViewModel itemPageViewModel;
            if (addedCommentId.HasValue)
            {
                itemPageViewModel = new ItemPageViewModel(
                    CreateItemPanelViewModel(item), item.Comments.Select(CreateCommentViewModel), addedCommentId);
            }
            else if (commentAddingException != null)
            {
                itemPageViewModel = new ItemPageViewModel(
                    CreateItemPanelViewModel(item), item.Comments.Select(CreateCommentViewModel), commentAddingException);
            }
            else
            {
                itemPageViewModel = new ItemPageViewModel(
                    CreateItemPanelViewModel(item), item.Comments.Select(CreateCommentViewModel));
            }
            itemPageViewModel.Sidebar = await PrepareSidebar();

            return itemPageViewModel;
        }

        private async Task<SidebarViewModel> PrepareSidebar()
        {
            var rankingDrivers = await _driversRepository.GetMostDownvotedDrivers(SidebarDriversRankingCount);
            var rankingDriverViewModels = rankingDrivers.Select(Mapper.Map<DriverRankingViewModel>);

            return new SidebarViewModel
            {
                DriversRanking = new DriversRankingViewModel
                {
                    Drivers = rankingDriverViewModels
                }
            };
        }
    }
}