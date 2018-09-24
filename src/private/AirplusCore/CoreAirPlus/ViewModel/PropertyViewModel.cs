using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreAirPlus.ViewModel
{
    public class PropertyViewModel
    {
        [HiddenInput]
        [Key]
        public int? PropertyId { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Listing Id")]
        [DataType(DataType.Text)]
        public long ListingId { get; set; }

        [Display(Name ="Property Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Display(Name = "ICalendar Address or URL")]
        [DataType(DataType.Url)]
        public string IcalUrl { get; set; }

        [HiddenInput]
        public int HostId { get; set; }

        [Display(Name ="Listings that you want to Know(Per Line one Listing)")]
        [DataType(DataType.MultilineText)]
        public string Listings { get; set; }
    }
}
