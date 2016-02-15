using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Driver.WebSite.Models;
using Driver.WebSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ApplicationDbContext = Driver.WebSite.Models.ApplicationDbContext;

namespace Driver.WebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(new HomeViewModel(await GetItems()));
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
            item.UpScore = 0;
            item.Author = user;
            item.DateAdded = DateTime.Now;

            _context.Items.Add(item);

            await _context.SaveChangesAsync();

            return View("Index", new HomeViewModel(await GetItems()) { DisplayAddedInfo = true });
        }

        private async Task<IEnumerable<ItemPanelViewModel>> GetItems(int? id = null)
        {
            var user = await this.GetCurrentUserAsync(_context);
            var query =
                from item in _context.Items
                orderby item.DateAdded descending
                select new { Item = item, AuthorLogin = item.Author.UserName };

            if (id != null)
            {
                query = query.Where(item => item.Item.Id == id);
            }

            var items = await query.ToArrayAsync();

            var itemsViewModels = items.Select(item =>
                {
                    var viewModel = Mapper.Map<Item, ItemPanelViewModel>(item.Item);
                    viewModel.AuthorLogin = item.AuthorLogin;
                    return viewModel;
                }).ToArray();

            if (user != null)
            {
                var ratesQuery =
                    from itemRate in _context.Set<ItemRate>()
                    where itemRate.User.Id == user.Id
                    select itemRate;

                var rates = await ratesQuery.ToArrayAsync();

                foreach (var itemViewModel in itemsViewModels)
                {
                    bool? userVotedPositive = rates.FirstOrDefault(r => r.Item.Id == itemViewModel.Id)?.Positive;
                    itemViewModel.UserVoting = userVotedPositive.HasValue ? (userVotedPositive.Value ? 1 : -1) : 0;
                }
            }

            return itemsViewModels;
        }

        [AllowAnonymous]
        public async Task<ActionResult> ItemPage(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var item = (await GetItems(id)).FirstOrDefault();

            if (item != null)
                return View(new ItemPageViewModel(item));
            else
                return HttpNotFound();
        }
    }
}