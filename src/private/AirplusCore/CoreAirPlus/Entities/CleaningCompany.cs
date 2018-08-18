using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAirPlus.Entities
{
    public class CleaningCompany
    {
        [Key]
        public int CleaningCompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }


        [ForeignKey("CleaningCompanyId")]
        public ICollection<Reservation> reservations { get; set; }
    }
}
