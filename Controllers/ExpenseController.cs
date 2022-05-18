using InAndOut.Data;
using InAndOut.Models;
using InAndOut.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InAndOut.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext dB;


        public ExpenseController(ApplicationDbContext db)
        {
            dB = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Expense> ObjList = dB.Expenses;

            foreach(var obj in ObjList)
            {
                obj.ExpenseType = dB.ExpenseTypes.FirstOrDefault(u => u.Id == obj.ExpenseTypeId);
            }
            
            return View(ObjList);
        }

        //GET - Create
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> TypeDropDown = _db.ExpenseTypes.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.TypeDropDown = TypeDropDown; 


            ExpenseVM expenseVM = new ExpenseVM()
            {
                Expense = new Expense(),
                TypeDropDown = dB.ExpenseTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            return View(expenseVM);
        }

        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseVM obj)
        {
            if (ModelState.IsValid)
            {
                //obj.ExpenseTypeId = 1;
                dB.Expenses.Add(obj.Expense);
                dB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
         
        }

        //GET Delete
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = dB.Expenses.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }


        //Post Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = dB.Expenses.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            dB.Expenses.Remove(obj);
            dB.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET Update
        public IActionResult Update(int? id)
        {

            ExpenseVM expenseVM = new ExpenseVM()
            {
                Expense = new Expense(),
                TypeDropDown = dB.ExpenseTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return NotFound();
            }

            expenseVM.Expense = dB.Expenses.Find(id);
            if (expenseVM.Expense == null)
            {
                return NotFound();
            }
            return View(expenseVM);

        }

        //POST - Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ExpenseVM obj)
        {
            if (ModelState.IsValid)
            {
                dB.Expenses.Update(obj.Expense);
                dB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

    }
}
