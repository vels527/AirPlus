using CoreAirPlus.Services;
using CoreAirPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoreAirPlus.Repositories
{
    public class SqlReadRepository : IReadRepository
    {
        private IDbReadService _db;
        public SqlReadRepository(IDbReadService db)
        {
            _db = db;
        }
        public CleaningCompany GetCleaning(int cleaningId)
        {
            var cleaningCompany = _db.GetWithIncludes<CleaningCompany>().Where(comp => comp.CleaningCompanyId == cleaningId).FirstOrDefault();
            return cleaningCompany;
        }
        public IEnumerable<CleaningCompany> GetCompanies()
        {
            return _db.GetWithIncludes<CleaningCompany>();
        }
        public Guest GetGuest(int gId)
        {
            var guest = _db.GetWithIncludes<Guest>().Where(g => g.GuestId == gId).FirstOrDefault();
            return guest;
        }
        public IEnumerable<Guest> GetGuests()
        {
            var guests = _db.GetWithIncludes<Guest>();
            return guests;
        }
        public Host GetHost(int hId)
        {
            var host = _db.GetWithIncludes<Host>().Where(c => c.HostId == hId).FirstOrDefault();
            return host;
        }
        public IEnumerable<Host> GetHosts()
        {
            return _db.GetWithIncludes<Host>();
        }
        public Property GetProperty(int propId)
        {
            var property = _db.GetWithIncludes<Property>().Where(p => p.PropertyId == propId).FirstOrDefault();
            return property;
        }
        public IEnumerable<Property> GetProperties()
        {
            return _db.GetWithIncludes<Property>();
        }
        public IEnumerable<Reservation> GetReservationsByGuestAndProperty(int guestid, int propertyid, DateTime checkin)
        {
            var guest = GetGuest(guestid);
            var Reservations = guest.reservations.Where(r => r.PropertyId == propertyid && r.CheckIn == checkin);
            return Reservations;
        }
        public IEnumerable<Reservation> GetReservationsByHost(int hId)
        {
            var host = GetHost(hId);
            var properties = host.properties;
            var Reservations = new List<Reservation>();
            foreach (Property p in properties)
            {
                var eachproperty = GetProperty(p.PropertyId);
                foreach (Reservation r in eachproperty.reservations)
                {
                    Reservations.Add(r);
                }
            }
            return Reservations;
        }
        public IEnumerable<Reservation> GetReservationsByHostAndDate(int hId, DateTime datestart, DateTime dateend)
        {
            var reservations = GetReservationsByHost(hId).Where<Reservation>(R => R.CheckIn >= datestart && R.CheckIn <= dateend);
            return reservations;
        }
        public IEnumerable<Reservation> GetReservationsByHostAndGuest(int hId, Guest guest)
        {
            var reservations = GetReservationsByHost(hId).Where<Reservation>(R => R.GuestId == guest.GuestId);
            return reservations;
        }

        public bool AuthenticateHost(string userName, string password)
        {
            var HostCount = _db.GetWithIncludes<Host>().Where(u => u.Username == userName && u.Password == password).Count();
            if (HostCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Host GetHost(string userId)
        {
            return _db.GetWithIncludes<Host>().Where(c => c.Username == userId).FirstOrDefault();
        }

        public bool UpdateReservation(Reservation reservation)
        {
           return _db.SaveReservation(reservation);
        }

        public IEnumerable<CalendarPrice> GetCalendarPrices()
        {
            return _db.GetWithIncludes<CalendarPrice>();
        }
    }
}