using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Security;
using System.Text;

namespace CoreAirPlus.Entities
{
    public class Host
    {
        [Key]
        public int HostId { get; set; }
        public string FullName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public byte? Age { get; set; }
        public DateTime? DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Remarks { get; set; }
        [Required]
        public string Password
        {
            get;set;
        }
        [Required]
        public string Username { get; set; }

        [ForeignKey("HostId")]
        public ICollection<Property> properties { get; set; }
    }
}
