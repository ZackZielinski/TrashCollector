﻿using System.ComponentModel.DataAnnotations;

namespace TrashCollector2.Models
{
    public class Days
    {
        public int Id { get; set; }

        [Display(Name = "Pickup Day")]
        public string DayName { get; set; }

        [Display(Name = "Weekly Cost:")]
        public float WeeklyPayment { get; set; }

    }
}