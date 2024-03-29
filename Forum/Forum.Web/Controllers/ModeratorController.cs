﻿using Autofac;
using Forum.Membership.Entities;
using Forum.Web.Models.Moderator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    [Authorize(Policy = "Moderator")]
    public class ModeratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ModeratorController> _logger;
        private readonly ILifetimeScope _scope;

        public ModeratorController(
            UserManager<ApplicationUser> userManager,
            ILifetimeScope scope,
            ILogger<ModeratorController> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _scope = scope;
        }

        public IActionResult CreateBoard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard(CreateBoardModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.CreateBoard(_userManager.GetUserName(User));
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Board Creation Failed");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Index()
        {
            var model = _scope.Resolve<BoardListModel>();
            model.LoadAllBoards();
            return View(model);
        }

        public IActionResult EditBoard(Guid id)
        {
            var model = _scope.Resolve<EditBoardModel>();
            model.LoadBoard(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBoard(EditBoardModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.EditBoard(_userManager.GetUserName(User));
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Board Creation Failed");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult DeleteBoard(Guid id)
        {
            var model = _scope.Resolve<DeleteBoardModel>();
            model.LoadBoard(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBoard(DeleteBoardModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.DeleteBoard(_userManager.GetUserName(User));
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.LogError(ex, "Board Creation Failed");
                    return View(model);
                }
            }
            return View(model);
        }
    }
}