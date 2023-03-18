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
    public class AnswerController : Controller
    {
        private readonly QuestionBankDBContext _context;
        public AnswerController(QuestionBankDBContext context)
        {
            _context = context;
        }
        // GET: AnswerController
        public async Task<IActionResult> Index()
        {
            var answers = await _context.Answers.ToListAsync();
            return View(answers);
        }

        // GET: AnswerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AnswerController/Create
        public async Task<IActionResult> AddOrEdit(String QuestionId)
        {
            ViewBag.PageName = QuestionId.ToString() == "" ? "Create Answer" : "Edit Answer";
            ViewBag.IsEdit = QuestionId.ToString() == "" ? false : true;
            if(QuestionId.ToString()== "")
            {
                return View();
            }
            else
            {
                var answer = await _context.Answers.FindAsync(QuestionId.ToString());
                if (answer == null)
                {
                    return NotFound();
                }
                return View(answer);
            }
        }

        // POST: AnswerController/Create
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

        // GET: AnswerController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: AnswerController/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(String id,[Bind("id,QuestionId,Content,Correct,UserId,UpdateDate")] Answer answerData)
        {
            bool isAnswerExist = false;
            Answer answer = await _context.Answers.FindAsync(id);
            if(answer != null)
            {
                isAnswerExist = true;
            }
            else
            {
                answer = new Answer();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    answer.QuestionId = answerData.QuestionId;
                    answer.Content = answerData.Content;
                    answer.Correct = answerData.Correct;
                    answer.UserId = answerData.UserId;
                    answer.UpdateDate = answerData.UpdateDate;

                    if (isAnswerExist)
                    {
                        _context.Update(answer);
                    }
                    else
                    {
                        _context.Add(answer);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(answerData);
        }

        // GET: AnswerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AnswerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
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
