using CRUD_AspNetCore5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_AspNetCore5.Controllers
{
    public class QuestionController : Controller
    {
        private readonly QuestionBankDBContext _context;

        public QuestionController(QuestionBankDBContext context)
        {
            _context = context;
        }
        // GET: QuestionController
        public async Task<IActionResult> Index()
        {

            var question = await _context.Questions.ToListAsync();
            return View(question);
        }

        // GET: QuestionController/Details/5
        public async Task<IActionResult> Details(Guid questionId)
        {
           if(questionId == Guid.Empty)
            {
                return NotFound();
            }
            var question = await _context.Questions.FirstOrDefaultAsync(m => m.Id == questionId);
            if(questionId == Guid.Empty)
            {
                return NotFound();
            }
            return View(question);
        }
        //AddOrEdit Get Mathod
        public async Task<IActionResult> AddOrEdit(Guid questionId)
        {
            ViewBag.PageName = questionId==Guid.Empty? "Create Question" : "Edit Question";
            ViewBag.IsEdit = questionId.ToString() == null ? false : true;
            if(questionId == Guid.Empty)
            {
                return View();
            }
            else
            {
                var question = await _context.Questions.FindAsync(questionId);

                if (question == null)
                {
                    return NotFound();
                }
                return View(question);
            }
        }

        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid questionId,[Bind("QuestionId,Content,UserId,UpdateDate")] Question questionData)
        {
            bool IsQuestionExist = false;

            Question question = await _context.Questions.FindAsync(questionId);
            if(question != null)
            {
                IsQuestionExist = true;
            }
            else
            {
                question = new Question();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    question.Content = questionData.Content;
                    question.UserId = questionData.UserId;
                    question.UpdateDate = questionData.UpdateDate;

                    if (IsQuestionExist)
                    {
                        _context.Update(question);
                    }
                    else
                    {
                        _context.Add(question);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(questionData);
        }
        //// GET: QuestionController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: QuestionController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: QuestionController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: QuestionController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: QuestionController/Delete/5
        public async Task<IActionResult> Delete(Guid? questionId)
        {
            if (questionId == Guid.Empty)
            {
                return NotFound();
            }
            var question = await _context.Questions.FirstOrDefaultAsync(m => m.Id == questionId);
            return View(question);
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
