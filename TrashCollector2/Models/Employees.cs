using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrashCollector2.Models
{
    public class Employees
    {
        public int Id { get; set; }

        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [ForeignKey("Userid")]
        public ApplicationUser Usertype { get; set; }

        public string Userid { get; set; }

        [Display(Name = "Zip Code:")]
        public string ZipCode { get; set; }

    }
}