using System.ComponentModel.DataAnnotations;

namespace TrashCollector2.Models
{
    public class Days
    {
        public int Id { get; set; }

        [Display(Name = "Pickup On")]
        public string DayName { get; set; }

        public float Payment { get; set; }

    }
}