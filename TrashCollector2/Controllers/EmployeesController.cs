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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                string userid = User.Identity.GetUserId();
                var employee = db.Employees.Find(userid);

                return View(employee);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        public ActionResult AvailablePickups()
        {
            string UserId = User.Identity.GetUserId();
            var CurrentEmployee = db.Employees.SingleOrDefault(w => w.Userid == UserId);

            var Pickups = db.Pickups.Include(x=>x.Customer).Include(y=>y.PickupDate).Where(z=>z.ZipCode == CurrentEmployee.ZipCode).ToList();


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
            var SelectedPickup = db.Pickups.Find(id);

            if(SelectedPickup == null)
            {
                return HttpNotFound();
            }

            return View(SelectedPickup);
        }


        public ActionResult MapRoute(string zipcode)
        {
            var AllPickups = GetDailyPickupList();

            var MatchedPickups = AllPickups.Where(y => y.ZipCode == zipcode).ToList();

            return View(MatchedPickups);
        }


        protected List<Pickup> GetDailyPickupList()
        {
            string Today = DateTime.Now.DayOfWeek.ToString();
            return db.Pickups.Include(x => x.Customer).Include(y => y.PickupDate).Where(z => z.PickupDate.DayName == Today).ToList();
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
