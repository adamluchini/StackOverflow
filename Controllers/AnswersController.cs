using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StackOverflow.Models;
using Microsoft.AspNetCore.Identity;

namespace StackOverflow.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db
        )
        {
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int id)
        {
            ViewData["questionId"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Answer answer)
        {
            var currentUser = await _userManager.FindByIdAsync(User.Identity.Name);
            answer.User = currentUser;
            _db.Answers.Add(answer);
            _db.SaveChanges();
            return RedirectToAction("Details", "Questions");
        }
    }
}
