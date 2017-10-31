using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrashCollector2.Models
{
    public class Pickup
    {
        public Pickup()
        {
            VacationStatus = false;
            PickupStatus = false;
        }

        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name ="City")]
        public string City { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "Please enter the abbrivation of your state")]
        [Display(Name ="State")]
        public string State { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "Please enter a proper Zip Code", MinimumLength = 5)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Vacation Status:")]
        public bool VacationStatus { get; set; }

        [ForeignKey("PickupDate")]
        public int DayId { get; set; }

        [Display(Name = "Pickup Every")]
        public Days PickupDate { get; set; }

        [Display(Name ="Pickup Status:")]
        public bool PickupStatus { get; set; }


        [Display(Name = "Monthly Payment:")]
        public float MonthlyPayment { get; set; }

        public IEnumerable<Days> Week { get; set; }
    }
}