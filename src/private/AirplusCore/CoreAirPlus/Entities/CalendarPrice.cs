using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreAirPlus.Entities
{
    public class CalendarPrice
    {

        public int? PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public virtual Property property { get; set; }

        [Key, Column(Order = 1)]
        public long ListingID { get; set; }

        [Key, Column(Order = 2)]
        public DateTime CalendarDate { get; set; }

        public bool IsAvailable { get; set; }

        public decimal Price { get; set; }
    }
}