﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TrashCollector2.Models;

namespace TrashCollector2.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        public ActionResult DailyPickups()
        {
            var Pickups = GetDailyPickupList();

            return View(Pickups);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            Employees NewEmployee = new Employees()
            {
                Userid = User.Identity.GetUserId()
            };
            return View(NewEmployee);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employees employees)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employees);
                db.SaveChanges();
                return RedirectToAction("DailyPickups");
            }

            return View(employees);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employees employees)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employees).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employees);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }

            db.Employees.Remove(employees);
            db.SaveChanges();

            return View(employees);
        }

        public ActionResult CustomerList(string zipcode)
        {
            var DailyPickupList = GetDailyPickupList();

            var PickupsFromZipCode = DailyPickupList.Where(z => z.ZipCode == zipcode).ToList();

            return View(PickupsFromZipCode);
        }


        public ActionResult MapView(string zipcode)
        {
            var AvailablePickups = GetDailyPickupList();

            return View(AvailablePickups);
        }

        public ActionResult ChangePickupStatus(int id)
        {
            var SelectedPickup = db.Pickups.SingleOrDefault(y => y.Id == id);

            UpdatePickupStatus(SelectedPickup);

            return RedirectToAction("CustomerList", new { zipcode = SelectedPickup.ZipCode });
        }


        protected List<Pickup> GetDailyPickupList()
        {
            string Today = DateTime.Now.DayOfWeek.ToString();
            return db.Pickups.Include(x => x.Customer).Include(y => y.PickupDate).Where(z => z.PickupDate.DayName == Today).ToList();
        }

        protected void UpdatePickupStatus(Pickup pickup)
        {
            if (pickup.PickupStatus == true)
            {
                pickup.PickupStatus = false;
            }
            else
            {
                pickup.PickupStatus = true;
            }
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
