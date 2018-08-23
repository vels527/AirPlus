using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace CoreAirPlus.Entities
{
    public class Reservation
    {
        [Key]
        [HiddenInput]
        public int GuestId { get; set; }

        [ForeignKey("GuestId")]
        public virtual Guest guest { get; set; }


        [Key]
        [HiddenInput]
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public virtual Property property { get; set; }

        [HiddenInput]
        public int? CleaningCompanyId { get; set; }
        [ForeignKey("CleaningCompanyId")]
        public virtual CleaningCompany CleaningCompany { get; set; }

        

        [Key]
        public DateTime CheckIn { get; set; }
        [Key]
        public DateTime CheckOut { get; set; }
        [Display(Name = "Requested Check In")]
        public DateTime? RCheckIn { get; set; }
        [Display(Name = "Requested Check Out")]
        public DateTime? RCheckOut { get; set; }
        [Display(Name = "Cleaning Time")]
        public DateTime? CleaningTime { get; set; }
        [HiddenInput]
        public DateTime CreatedTime { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
    }
}