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
            Customers customers = db.Customers.Include(x=>x.PickupDate).SingleOrDefault(y=>y.Id == id);
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
            Customers NewCustomer = new Customers()
            {
                Week = db.Week.ToList()
            };
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
            Customers customers = db.Customers.Include(x => x.PickupDate).SingleOrDefault(y => y.Id == id);
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
            Customers customers = db.Customers.Include(y => y.PickupDate).SingleOrDefault(x => x.Id == id);
            if (customers == null)
            {
                customers = GetCustomerFromUserId();
            }

            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PickupProgress()
        {
            var customer = GetCustomerFromUserId();

            return View(customer);
        }

        protected Customers GetCustomerFromUserId()
        {
            string UserId = User.Identity.GetUserId();
            var customer = db.Customers.Include(x => x.PickupDate).SingleOrDefault(y => y.Userid == UserId);

            return customer;
        }

        protected void CalculateCustomerPayment(Customers customer)
        {
            customer.MonthlyPayment = (4 * customer.PickupDate.Payment);
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
