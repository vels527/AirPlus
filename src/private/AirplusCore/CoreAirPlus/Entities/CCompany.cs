using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAirPlus.Entities
{
    public class CCompany
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }

        [ForeignKey("CCId")]
        public ICollection<Reservation> reservations { get; set; }
    }
}
