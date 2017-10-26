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

            return View(customers);
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
                return RedirectToAction("Index", "Home");
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
                NewPickup.DayId = NewPickup.PickupDate.Id;
                db.Pickups.Add(NewPickup);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("PickupProgress");
        }

        public ActionResult PickupProgress()
        {
            var customer = GetCustomerFromUserId();
            var CustomerPickups = db.Pickups.Include(y => y.PickupDate).Where(x => x.CustomerId == customer.Id).ToList();

            return View(CustomerPickups);
        }

        public ActionResult ChangeStatus(int id)
        {
            var SelectedPickup = db.Pickups.Find(id);

            CheckStatus(SelectedPickup);

            return RedirectToAction("PickupProgress");
        }

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


        protected Customers GetCustomerFromUserId()
        {
            string UserId = User.Identity.GetUserId();
            var customer = db.Customers.SingleOrDefault(y => y.Userid == UserId);

            return customer;
        }

        protected void CalculateCustomerPayment(Customers customer)
        {
            var Pickups = db.Pickups.Where(x => x.CustomerId == customer.Id).ToList();
            float Payment = 0;

            foreach(Pickup collection in Pickups)
            {
                Payment += (4 * collection.PickupDate.Payment);
            }

            customer.MonthlyPayment = Payment;
            db.SaveChanges();
        }

        protected void CheckStatus(Pickup SelectedPickup)
        {
            if (SelectedPickup.ActiveStatus == true)
            {
                SelectedPickup.ActiveStatus = false;
            }
            else
            {
                SelectedPickup.ActiveStatus = true;
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
