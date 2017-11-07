using Microsoft.AspNet.Identity;
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
        public ActionResult Details()
        {
            string userid = User.Identity.GetUserId();
            var employee = db.Employees.SingleOrDefault(x=>x.Userid == userid);

            return View(employee);
        }

        public ActionResult AvailablePickups()
        {
            WeeklyReset();
            string UserId = User.Identity.GetUserId();
            var CurrentEmployee = db.Employees.SingleOrDefault(w => w.Userid == UserId);

            if (CurrentEmployee == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Pickups = GetDailyPickupList();
            var PickupsInZipCode = Pickups.Where(x => x.ZipCode == CurrentEmployee.ZipCode).ToList();

            return View(PickupsInZipCode);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            Employees NewEmployee = new Employees();
            return View(NewEmployee);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employees employees)
        {
            if (ModelState.IsValid)
            {
                employees.Userid = User.Identity.GetUserId();
                db.Employees.Add(employees);
                db.SaveChanges();
                return RedirectToAction("AvailablePickups");
            }

            return View(employees);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            var employee = db.Employees.Find(id);

            if(employee == null)
            {
                string UserId = User.Identity.GetUserId();
                employee = db.Employees.SingleOrDefault(y => y.Userid == UserId);
            }

            return View(employee);
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
                return RedirectToAction("AvailablePickups");
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

        public ActionResult PickupOrder(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var PickupList = GetDailyPickupList();

            var SelectedPickup = PickupList.SingleOrDefault(z => z.Id == id);
                        
            return View(SelectedPickup);
        }


        public ActionResult MapView(int? id)
        {
            var SelectedPickup = db.Pickups.Include(y=>y.Customer).SingleOrDefault(z=>z.Id == id);

            return View(SelectedPickup);
        }

        public ActionResult MapRoute(string zipcode)
        {
            var DailyPickups = GetDailyPickupList();

            var PickupsInZipCode = DailyPickups.Where(x => x.ZipCode == zipcode).ToList();

            return View(PickupsInZipCode);
        }


        public ActionResult VacationDates(int? id)
        {
            var SelectedPickup = db.Pickups.Find(id);

            if(SelectedPickup == null)
            {
                return HttpNotFound();
            }

            return View(SelectedPickup);
        }

        protected List<Pickup> GetDailyPickupList()
        {
            string Today = DateTime.Now.DayOfWeek.ToString();
            return db.Pickups.Include(x => x.Customer).Include(y => y.PickupDate).Where(z => z.PickupDate.DayName == Today).ToList();
        }

        protected void WeeklyReset()
        {
            var AllPickups = db.Pickups.ToList();
            string TodaysDate = DateTime.Now.DayOfWeek.ToString();
            string TodaysTime = DateTime.Now.ToString("t");

            if (TodaysDate == "Sunday" && TodaysTime == "11:59 PM")
            {
                foreach (var collection in AllPickups)
                {
                    collection.PickupStatus = false;
                    collection.VacationStatus = false;
                }
                db.SaveChanges();
            }
        }

        public ActionResult UpdatePickupStatus(int id)
        {
            var CurrentPickup = db.Pickups.Find(id);

            if (CurrentPickup.PickupStatus == true)
            {
                CurrentPickup.PickupStatus = false;
            }
            else
            {
                CurrentPickup.PickupStatus = true;
            }
            db.SaveChanges();

            return RedirectToAction("PickupOrder", new { id = CurrentPickup.Id });
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
