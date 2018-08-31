using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CoreAirPlus.Entities
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        [Required]
        public long ListingId { get; set; }
        [Required]
        public string Address { get; set; }
        public string IcalUrl { get; set; }

        [ForeignKey("HostId")]
        public int HostId { get; set; }
        public virtual Host host { get; set; }

        [ForeignKey("PropertyId")]
        public ICollection<Reservation> reservations { get; set; }

        [ForeignKey("PropertyId")]
        public ICollection<CalendarPrice> CalendarPrices { get; set; }
    }
}