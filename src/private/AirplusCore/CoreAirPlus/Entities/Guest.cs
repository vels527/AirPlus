using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAirPlus.Entities
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }
        [Required]
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Remarks { get; set; }
        public string Tag { get; set; }

        [ForeignKey("GuestId")]
        public ICollection<Reservation> reservations { get; set; }
    }
}
