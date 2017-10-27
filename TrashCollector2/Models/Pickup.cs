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

        [Display(Name = "Address:")]
        public string Location { get; set; }

        [Display(Name = "Zip Code:")]
        public string ZipCode { get; set; }

        [Display(Name = "Vacation Status:")]
        public bool VacationStatus { get; set; }

        [ForeignKey("PickupDate")]
        public int DayId { get; set; }

        [Display(Name ="Pickup Every:")]
        public Days PickupDate { get; set; }

        [Display(Name ="Pickup Status:")]
        public bool PickupStatus { get; set; }

        public IEnumerable<Days> Week { get; set; }
    }
}