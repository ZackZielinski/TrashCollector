using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TrashCollector2.Models;

namespace TrashCollector2.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var Customers = db.Customers.ToList();

            foreach(var customer in Customers)
            {
                CalculateNumberOfPickups(customer);
            }

            return View(Customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            Customers customers = db.Customers.SingleOrDefault(y=>y.Id == id);
            if (customers == null)
            {
                customers = GetCustomerFromUserId();
            }

            CalculateCustomerPayment(customers);

            var CustomerPickups = db.Pickups.Include(x => x.Customer).Include(y => y.PickupDate).Where(z => z.CustomerId == customers.Id).ToList();

            return View(CustomerPickups);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            Customers NewCustomer = new Customers();
            return View(NewCustomer);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customers customers)
        {
            if (ModelState.IsValid)
            {
                customers.Userid = User.Identity.GetUserId();
                db.Customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("NewPickup", "Customers");
            }

            return View(customers);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            Customers customers = db.Customers.SingleOrDefault(y => y.Id == id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customers customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                CalculateCustomerPayment(customer);
                return RedirectToAction("Details");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            Customers customers = db.Customers.SingleOrDefault(x => x.Id == id);
            if (customers == null)
            {
                customers = GetCustomerFromUserId();
            }

            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NewPickup()
        {
            var customer = GetCustomerFromUserId();

            Pickup NewPickup = new Pickup()
            {
                CustomerId = customer.Id,
                Week = db.Week.ToList()
            };

            return View(NewPickup);
        }

        [HttpPost]
        public ActionResult NewPickup(Pickup NewPickup)
        {

            if (ModelState.IsValid)
            {
                db.Pickups.Add(NewPickup);
                db.SaveChanges();
            }

            return RedirectToAction("PickupProgress");
        }

        //View Pickups
        public ActionResult PickupProgress()
        {
            WeeklyReset();
            var customer = GetCustomerFromUserId();

            if(customer == null)
            {
                return HttpNotFound();
            }

            CalculateCustomerPayment(customer);

            var CustomerPickups = db.Pickups.Include(y => y.PickupDate).Where(x => x.CustomerId == customer.Id).ToList();

            return View(CustomerPickups);
        }

        public ActionResult ChangeVacationDates(int id)
        {
            var SelectedPickup = db.Pickups.Find(id);

            return View(SelectedPickup);
        }

        [HttpPost]
        public ActionResult ChangeVacationDates(Pickup NewVacationTime)
        {
            var PickupInDb = db.Pickups.Find(NewVacationTime.Id);

            PickupInDb.VacationStart = NewVacationTime.VacationStart;
            PickupInDb.VacationEnd = NewVacationTime.VacationEnd;
            db.SaveChanges();

            CalculateVacationStatus(PickupInDb.Id);

            return RedirectToAction("PickupProgress");
        }

        // Delete Pickup
        public ActionResult DropPickup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var SelectedPickup = db.Pickups.SingleOrDefault(x => x.Id == id);

            if(SelectedPickup == null)
            {
                return HttpNotFound();
            }

            db.Pickups.Remove(SelectedPickup);
            db.SaveChanges();

            return RedirectToAction("PickupProgress");
        }

        public ActionResult EditPickup(int? id)
        {
            Pickup SelectedPickup = db.Pickups.SingleOrDefault(y => y.Id == id);
            if (SelectedPickup == null)
            {
                return HttpNotFound();
            }
            SelectedPickup.Week = db.Week.ToList();
            return View(SelectedPickup);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPickup(Pickup pickup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pickup).State = EntityState.Modified;
                db.SaveChanges();
            }

            var ChangedPickup = db.Pickups.Find(pickup.Id);

            CalculateVacationStatus(ChangedPickup.Id);

            return RedirectToAction("PickupProgress");
        }


        public ActionResult CustomerOrders(int id)
        {
            var CurrentCustomer = db.Customers.Find(id);
            var CustomerPickups = db.Pickups.Include(w=>w.PickupDate).Include(x => x.Customer).Where(y => y.CustomerId == id).ToList();

            if(CustomerPickups == null)
            {
                return HttpNotFound();
            }

            if(CurrentCustomer == null)
            {
                return HttpNotFound();
            }

            CalculateCustomerPayment(CurrentCustomer);

            return View(CustomerPickups);
        }


        protected Customers GetCustomerFromUserId()
        {
            string UserId = User.Identity.GetUserId();
            var customer = db.Customers.SingleOrDefault(y => y.Userid == UserId);

            return customer;
        }

        protected void CalculateCustomerPayment(Customers customer)
        {
            var Pickups = db.Pickups.Include(y => y.PickupDate).Where(x => x.CustomerId == customer.Id).ToList();
            float CalculatedMonthlyPayment = 0;
            float CalculatedTotal = 0;

            foreach (Pickup collection in Pickups)
            {
                CalculatedMonthlyPayment = (4 * collection.PickupDate.WeeklyPayment);
                if (collection.VacationStatus != true)
                {
                    CalculatedTotal += CalculatedMonthlyPayment;
                }
                collection.MonthlyPayment = CalculatedMonthlyPayment;
                db.SaveChanges();
            }

            customer.TotalPayment = CalculatedTotal;
            db.SaveChanges();
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
            }

            db.SaveChanges();
        }

        protected void CalculateVacationStatus(int PickupId)
        {
            var SelectedPickup = db.Pickups.Find(PickupId);
            var CurrentDate = DateTime.Today;
            var VacationBegin = Convert.ToDateTime(SelectedPickup.VacationStart);
            var VacationEnd = Convert.ToDateTime(SelectedPickup.VacationEnd);

            int VacationStartDateResult = DateTime.Compare(CurrentDate, VacationBegin);
            int VacationEndDateResult = DateTime.Compare(CurrentDate, VacationEnd);

            if(VacationStartDateResult == 1 || VacationStartDateResult == 0)
            {
                SelectedPickup.VacationStatus = true;
            }
            if(VacationEndDateResult == 1)
            {
                SelectedPickup.VacationStatus = false;
            }
           
            db.SaveChanges();
        }

        protected void CalculateNumberOfPickups(Customers CurrentCustomer)
        {
            var CustomerPickupList = db.Pickups.Include(x => x.Customer).Where(y => y.CustomerId == CurrentCustomer.Id).ToList();
            int TotalAssignedPickups = 0;
            int TotalActivePickups = 0;

            foreach (var Pickup in CustomerPickupList)
            {
                TotalAssignedPickups++;
                if (Pickup.VacationStatus == false)
                {
                    TotalActivePickups++;
                }
                Pickup.Customer.NumberOfAssignedPickups = TotalAssignedPickups;
                Pickup.Customer.NumberOfActivePickups = TotalActivePickups;
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
