using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrashCollector2.Models
{
    public class Customers
    {
        public Customers()
        {
            OnVacation = false;
        }

        public int Id { get; set; }

        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [ForeignKey("Userid")]
        public ApplicationUser Usertype { get; set; }

        public string Userid { get; set; }


        [Display(Name = "On Vacation")]
        public bool OnVacation { get; set; }

        [Display(Name = "Address:")]
        public string Location { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [ForeignKey("PickupDate")]
        public int DayId { get; set; }

        public Days PickupDate { get; set; }

        public float MonthlyPayment { get; set; }

        [Display(Name = "Pickup Status")]
        public bool IsTrashedPickedUp { get; set; }

        public IEnumerable<Days> Week { get; set; }

    }
}