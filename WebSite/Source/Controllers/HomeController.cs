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
            var itemsViewModels = items.Select(item =>
                {
                    var viewModel = Mapper.Map<Item, ItemPanelViewModel>(item);
                    viewModel.AuthorLogin = item.Author.UserName;
                    bool? userVotedPositive = item.Votes.FirstOrDefault()?.Positive;
                    viewModel.UserVoting = userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;
                    return viewModel;
                }).ToArray();

            return itemsViewModels;
        }

        [AllowAnonymous]
        public async Task<ActionResult> ItemPage(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var item = (await _itemsRepository.GetItem(id.Value, this.GetCurrentUserId()));

            if (item != null)
                return View(new ItemPageViewModel((CreateItemPanelViewModels(new[] { item })).First(), item.Comments));
            else
                return HttpNotFound();
        }
    }
}