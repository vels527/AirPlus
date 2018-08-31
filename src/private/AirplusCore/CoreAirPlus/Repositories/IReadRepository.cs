using System;
using System.Collections.Generic;
using CoreAirPlus.Entities;
using System.Threading.Tasks;

namespace CoreAirPlus.Repositories
{
    public interface IReadRepository
    {
        CleaningCompany GetCleaning(int cleaningId);
        IEnumerable<CleaningCompany> GetCompanies();
        Guest GetGuest(int gId);
        IEnumerable<Guest> GetGuests();
        Host GetHost(int hId);
        Host GetHost(string userId);
        IEnumerable<Host> GetHosts();
        Property GetProperty(int propId);
        IEnumerable<Property> GetProperties();
        IEnumerable<Reservation> GetReservationsByHost(int hId);
        IEnumerable<Reservation> GetReservationsByHostAndDate(int hId, DateTime datestart, DateTime dateend);
        IEnumerable<Reservation> GetReservationsByHostAndGuest(int hId, Guest guest);
        IEnumerable<Reservation> GetReservationsByGuestAndProperty(int guestid, int propertyid, DateTime checkin);
        bool AuthenticateHost(string userName,string password);
        bool UpdateReservation(Reservation reservation);
        IEnumerable<CalendarPrice> GetCalendarPrices();
    }
}
