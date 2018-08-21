using System.ComponentModel.DataAnnotations;
using System;

namespace CoreAirPlus.ViewModel
{
    public class ReservationViewModel
    {
        [Required]
        public string GuestName { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? RCheckIn { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? RCheckOut { get; set; }
        public string Remarks { get; set; }
        public DateTime? CleaningTime { get; set; }
        public string Status { get; set; }
    }
}
