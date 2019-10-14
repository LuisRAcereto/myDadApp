using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myDadApp.Models;

namespace myDadApp.Controllers
{
    public class UsersController : Controller
    {

        private readonly UserManager<MyAppUser> _userManager;
        private readonly myDataContext _context;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : MyAppUser
        {
        }

        public UsersController(
            myDataContext context,
            UserManager<MyAppUser> userManager,
            SignInManager<MyAppUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = _userManager.Users;
            foreach (var user in users)
            {
                user.ChoreCnt = _context.Chore.Where(c => c.Owner == user.UserName).Count();
            }
            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var User = await _userManager.FindByIdAsync(id);

            return View(User);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}