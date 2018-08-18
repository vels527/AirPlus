using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int CleaningCompanyId { get; set; }
        [ForeignKey("CleaningCompanyId")]
        public virtual CleaningCompany CleaningCompany { get; set; }

        

        [Key]
        public DateTime CheckIn { get; set; }
        [Key]
        public DateTime CheckOut { get; set; }
        public DateTime? RCheckIn { get; set; }
        public DateTime? RCheckOut { get; set; }
        public DateTime? CleaningTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Remarks { get; set; }
        public string status { get; set; }
    }
}