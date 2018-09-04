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
        public long ListingId { get; set; }//owner listingid
        //List of competitions listingid's
        [Required]
        public string Address { get; set; }
        public string IcalUrl { get; set; }

        [ForeignKey("HostId")]
        public int HostId { get; set; }
        public virtual Host host { get; set; }

        [ForeignKey("PropertyId")]
        public ICollection<Reservation> reservations { get; set; }
        //reservations should be Reservations
        [ForeignKey("PropertyId")]
        public ICollection<Listing> Listings { get; set; }
    }
}