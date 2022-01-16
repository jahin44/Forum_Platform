using Autofac;
using Forum.Web.Models;
using Forum.Web.Models.Home;
using Forum.Web.Models.Moderator;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILifetimeScope _scope;
        public HomeController(ILogger<HomeController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        public IActionResult Index()
        {
            var model = _scope.Resolve<BoardListModel>();
            model.LoadAllBoards();
            return View(model);
        }

        public IActionResult Topics(Guid id)
        {
            var model = _scope.Resolve<AllTopicsModel>();
            model.LoadTopics(id);
            return View(model);
        }

        public IActionResult Posts(Guid id)
        {
            var model = _scope.Resolve<AllPostsModel>();
            model.Load(id);
            return View(model);
        }

        public IActionResult Comments(Guid id)
        {
            var model = _scope.Resolve<AllCommentsModel>();
            model.Load(id);
            return View(model);
        }

        public async Task<IActionResult> User(Guid id)
        {
            var model = _scope.Resolve<UserPostsModel>();
            try
            {
                await model.Load(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No user for showing post.");
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}