using Microsoft.AspNet.Identity;
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
            return View(db.Customers.ToList());
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
            var customer = GetCustomerFromUserId();

            if(customer == null)
            {
                return HttpNotFound();
            }
            
            var CustomerPickups = db.Pickups.Include(y => y.PickupDate).Where(x => x.CustomerId == customer.Id).ToList();

            return View(CustomerPickups);
        }

        public ActionResult ChangeVacationStatus(int id)
        {
            var SelectedPickup = db.Pickups.Find(id);

            CheckStatus(SelectedPickup);

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
            return RedirectToAction("PickupProgress");
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

            foreach(Pickup collection in Pickups)
            {
                CalculatedMonthlyPayment = (4 * collection.PickupDate.Payment);
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

        protected void CheckStatus(Pickup SelectedPickup)
        {
            if (SelectedPickup.VacationStatus == true)
            {
                SelectedPickup.VacationStatus = false;
            }
            else
            {
                SelectedPickup.VacationStatus = true;
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
