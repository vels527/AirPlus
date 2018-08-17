using System;
using CoreAirPlus.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore;

namespace CoreAirPlus.Entities
{
    public class Reservation
    {
        [Key]
        public int GuestId { get; set; }

        [ForeignKey("GuestId")]
        public virtual Guest guest { get; set; }


        [Key]
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public virtual Property property { get; set; }

        public int CCId { get; set; }
        [ForeignKey("CCId")]
        public virtual CCompany cCompany { get; set; }

        

        [Key]
        public DateTime CheckIn { get; set; }
        [Key]
        public DateTime CheckOut { get; set; }
        public DateTime? RCheckIn { get; set; }
        public DateTime? RCheckOut { get; set; }
        public DateTime? CleaningTiming { get; set; }
        public DateTime CreateTiming { get; set; }
        public string Remarks { get; set; }
        public StatusCode status { get; set; }
    }
}