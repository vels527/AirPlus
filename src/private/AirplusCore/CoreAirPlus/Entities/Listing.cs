using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CoreAirPlus.Entities
{
    public class Listing
    {
        [Key]
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public virtual Property property { get; set; }

        [Key]
        public long ListingId { get; set; }
        [ForeignKey("ListingId")]
        public virtual List<CalendarPrice> CalendarDetail { get; set; }
    }
}