using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoreAirPlus.ViewModel
{
    public class ReservationViewModel
    {
        [Key]
        [HiddenInput]
        public int GuestId { get; set; }
        [Key]
        [HiddenInput]
        public int PropertyId { get; set; }
        
        [Required(ErrorMessage ="Guest Name is Required")]
        [Display(Name = "Guest Name")]
        [DataType(DataType.Text)]
        public string GuestName { get; set; }

        [Key]
        [Display(Name = "Check In")]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Display(Name = "Check Out")]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        [Display(Name = "Requested Check In")]
        [DataType(DataType.Time)]
        public string RCheckIn { get; set; }

        [Display(Name = "Requested Check Out")]
        [DataType(DataType.Time)]
        public string RCheckOut { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Cleaning Time")]
        [DataType(DataType.Time)]
        public DateTime? CleaningTime { get; set; }

        [Display(Name = "Status")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
    }
}
