﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWorkingEnvironment.Data;
using MyWorkingEnvironment.Models;
using MyWorkingEnvironment.Repository;

namespace MyWorkingEnvironment.Controllers
{
    public class ClockingController : Controller
    {
        private ClockingRepository _clockingRepository;
        private EmployeeRepository _employeeRepository;

        public ClockingController(ApplicationDbContext dbContext)
        {
            _employeeRepository = new EmployeeRepository(dbContext);
            _clockingRepository = new ClockingRepository(dbContext);
        }

        // GET: ClockingController
        public ActionResult Index()
        {
            var list = _clockingRepository.GetAllClockings();
            return View(list);
        }

        // GET: ClockingController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _clockingRepository.GetClokingById(id);
            return View("DetailsClocking", model);
        }

        // GET: ClockingController/Create
        public ActionResult Create()
        {
            var employees = _employeeRepository.GetAllEmployees();
            var employeeList = employees.Select(x => new SelectListItem(x.FirstName, x.IdEmployee.ToString()));
            ViewBag.EmployeeList = employeeList;
            return View("CreateClocking");
        }

        // POST: ClockingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var model = new ClockingModel();
                // IdEmployee trebuie sa vina in functie de cine este logat pe cont
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _clockingRepository.InsertClocking(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("CreateClocking");
            }
        }

        // GET: ClockingController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _clockingRepository.GetClokingById(id);
            return View("EditClocking", model);
        }

        // POST: ClockingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new ClockingModel();
                var task = TryUpdateModelAsync(model);
                model.IdClocking = id;
                task.Wait();
                if (task.Result)
                {
                    _clockingRepository.UpdateCloking(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Edit", id);
            }
        }

        // GET: ClockingController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _clockingRepository.GetClokingById(id);
            return View("DeleteClocking", model);
        }

        // POST: ClockingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _clockingRepository.DeleteClocking(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete", id);
            }
        }
    }
}
