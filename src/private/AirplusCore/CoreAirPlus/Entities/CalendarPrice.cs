using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreAirPlus.Entities
{
    public class CalendarPrice
    {

        [Key, Column(Order = 1)]
        public long ListingId { get; set; }//ID is Id

        [ForeignKey("ListingId")]
        public virtual Listing ListingDetail { get; set; }

        [Key, Column(Order = 2)]
        public DateTime CalendarDate { get; set; }

        public bool IsAvailable { get; set; }

        public decimal Price { get; set; }
    }
}