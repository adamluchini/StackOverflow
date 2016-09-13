using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StackOverflow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StackOverflow.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db
        )
        {
            _userManager = userManager;
            _db = db;
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        public IActionResult Index()
        {
            //var currentUser = await _userManager.FindByIdAsync(User.GetUserId());
            return View(_db.Questions.ToList());
        }

        public IActionResult Details(int id)
        {
            var questionList = _db.Questions.Where(x => x.QuestionId == id).Include(question => question.Answers).ToList();
            //var questionList = _db.Questions.ToList();
            return View(questionList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            var currentUser = await _userManager.FindByIdAsync(User.Identity.Name);
            question.User = currentUser;
            _db.Questions.Add(question);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var thisQuestion = db.Questions.FirstOrDefault(questions => questions.QuestionId == id);
            return View(thisQuestion);
        }

        [HttpPost]
        public IActionResult Edit(Question question)
        {
            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var thisQuestion = db.Questions.FirstOrDefault(x => x.QuestionId == id);
            db.Questions.Remove(thisQuestion);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
