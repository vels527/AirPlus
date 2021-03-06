﻿using CoreAirPlus.Entities;
using CoreAirPlus.Repositories;
using CoreAirPlus.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreAirPlus.Controllers
{

    public class HomeController : Controller
    {
        private IReadRepository _readRepository;
        public HomeController(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        [Route("")]
        [Route("Index")]
        public ActionResult Index()
        {
            int? CurrentThisHostId = HttpContext.Session.GetInt32("HostId");
            int hostid = CurrentThisHostId == null ? -1 : CurrentThisHostId.Value;
            if (hostid == -1)
            {
                return RedirectToPage("/Login");
            }
            var properties = _readRepository.GetHost(hostid).properties;
            if (properties.Count == 0)
            {
                return RedirectToAction("Property");
            }
            var reservations = _readRepository.GetReservationsByHost(hostid);
            var reservationViewModel = from c in reservations select new ReservationViewModel
            {
                GuestId = c.GuestId,
                PropertyId = c.PropertyId,
                GuestName =_readRepository.GetGuest(c.GuestId).FullName,
                CheckIn = c.CheckIn,
                CheckOut = c.CheckOut,
                RCheckIn = c.RCheckIn == null ? DateTime.MinValue.AddHours(16).ToShortTimeString() : c.RCheckIn.Value.ToShortTimeString(),
                RCheckOut = c.RCheckOut == null ? DateTime.MinValue.AddHours(11).ToShortTimeString() : c.RCheckOut.Value.ToShortTimeString(),
                Remarks = c.Remarks,
                CleaningTime = c.CleaningTime,
                Status = c.status
            };
            return View(reservationViewModel);
        }

        [HttpGet("Property")]
        public ActionResult Property()
        {
            return View();
        }

        [Route("LoginDirect")]
        public ActionResult LoginDirect()
        {
            return RedirectToPage("/Login");
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            int? CurrentThisHostId = HttpContext.Session.GetInt32("HostId");
            int hostid = CurrentThisHostId == null ? -1 : CurrentThisHostId.Value;
            if (hostid == -1)
            {
                return RedirectToPage("/Login");
            }
            var reservations = _readRepository.GetReservationsByHost(hostid).FirstOrDefault();
            var reservationViewModel = new ReservationViewModel {
                GuestId=reservations.GuestId,
                PropertyId=reservations.PropertyId,
            };
            return View(reservationViewModel);

            //return View();
        }

        // POST: Home/Create
        [HttpPost("Create")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ReservationViewModel collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        [HttpGet("Edit")]
        public ActionResult Edit(int guestid,int propertyid,DateTime checkIn)
        {
            var reservations = _readRepository.GetReservationsByGuestAndProperty(guestid,propertyid,checkIn);
            var reservationViewModels = from c in reservations
                                       select new ReservationViewModel
                                       {
                                           GuestId = c.GuestId,
                                           PropertyId = c.PropertyId,
                                           CleaningCompanyId=c.CleaningCompanyId,
                                           GuestName = _readRepository.GetGuest(c.GuestId).FullName,
                                           CheckIn = c.CheckIn,
                                           CheckOut = c.CheckOut,
                                           RCheckIn = c.RCheckIn == null ? DateTime.MinValue.AddHours(16).ToShortTimeString() : c.RCheckIn.Value.ToShortTimeString(),
                                           RCheckOut = c.RCheckOut == null ? DateTime.MinValue.AddHours(11).ToShortTimeString() : c.RCheckOut.Value.ToShortTimeString(),
                                           Remarks = c.Remarks,
                                           CleaningTime = c.CleaningTime,
                                           Status = c.status
                                       };
            return View(reservationViewModels.FirstOrDefault());
        }

        [HttpGet("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public ActionResult Register(HostViewModel hostvm)
        {
            if (ModelState.IsValid)
            {
                Host host = new Host();
                host.FirstName = hostvm.FirstName;
                host.LastName = hostvm.LastName;
                host.FullName = hostvm.FirstName + " " + hostvm.LastName;
                host.Password = hostvm.Password;
                host.Email = hostvm.Email;
                host.Username = hostvm.Email;
                var checkIfExists = _readRepository.GetHost(host.Username);
                if (checkIfExists is null)
                {
                    if (_readRepository.SaveHost(host))
                    {
                        Task.Delay(200).Wait();
                        Host verifiedHost = _readRepository.GetHost(host.Username);
                        HttpContext.Session.SetInt32("HostId", host.HostId);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("","Kindly Try Again or contact support,Some Technical Issue");
                    }
                }
                else
                {
                    ModelState.AddModelError("","UserId Already Present");
                }
            }
            return BadRequest(ModelState);
        }

        /*This is specifically for edit time span*/
        private TimeSpan StringToTime(string InOutime)
        {
            string checkintime = InOutime;
            TimeSpan RCheckIn = TimeSpan.MinValue;
            if (String.IsNullOrEmpty(checkintime))
            {
                RCheckIn = new TimeSpan(0,0,0);
            }
            else
            {
                
                int hours = Convert.ToInt32(checkintime.Split(":")[0]);
                int minutes = Convert.ToInt32(checkintime.Split(":")[1]);
                RCheckIn = new TimeSpan(hours, minutes, 0);
            }
            return RCheckIn;
        }

        // POST: Home/Edit/5
        [HttpPost("Edit")]
        public ActionResult Edit(ReservationViewModel reservationvm)
        {
            try
            {
                // TODO: Add update logic here

                TimeSpan TimeCheckIn = StringToTime(reservationvm.RCheckIn);
                TimeSpan TimeCheckOut = StringToTime(reservationvm.RCheckOut);
                if (ModelState.IsValid)
                {
                   
                    
                    Reservation reservation = new Reservation {
                        GuestId = reservationvm.GuestId,
                        PropertyId = reservationvm.PropertyId,
                        CleaningCompanyId = reservationvm.CleaningCompanyId,
                        CheckIn = reservationvm.CheckIn,
                        CheckOut=reservationvm.CheckOut,
                        RCheckIn=reservationvm.CheckIn.Add(TimeCheckIn),
                        RCheckOut=reservationvm.CheckOut.Add(TimeCheckOut),
                        CleaningTime=reservationvm.CleaningTime,
                        Remarks=reservationvm.Remarks,
                        status=reservationvm.Status
                    };
                    _readRepository.UpdateReservation(reservation);
                }
                else
                {
                    //ModelState.AddModelError()
                    return View(reservationvm);
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("Calendar")]
        public ActionResult CalendarDays()
        {
            int? CurrentThisHostId = HttpContext.Session.GetInt32("HostId");
            int hostid = CurrentThisHostId == null ? -1 : CurrentThisHostId.Value;
            if (hostid == -1)
            {
                return RedirectToPage("/Login");
            }
            //var calendars = _readRepository.GetCalendarPrices();
            var properties = _readRepository.GetHost(hostid).properties;
            List<CalendarPrice> calendarprices = new List<CalendarPrice>();
            foreach (var property in properties)
            {
                calendarprices.AddRange(_readRepository.GetCalendarPrices(property.PropertyId));
            }            
            //calendarprices = calendars.ToList();
            for(int i = 0; i < calendarprices.Count(); i++)
            {
                for(int j = i+1; j < calendarprices.Count(); j++)
                {
                    if(calendarprices[i].ListingId> calendarprices[j].ListingId)
                    {
                        var temp =calendarprices[i];
                        calendarprices[i] = calendarprices[j];
                        calendarprices[j] = temp;
                    }
                }
            }
            for (int i = 0; i < calendarprices.Count(); i++)
            {
                for (int j = i + 1; j < calendarprices.Count(); j++)
                {
                    if (calendarprices[i].ListingId == calendarprices[j].ListingId)
                    {
                        if (calendarprices[i].CalendarDate>calendarprices[j].CalendarDate)
                        {
                            var temp = calendarprices[i];
                            calendarprices[i] = calendarprices[j];
                            calendarprices[j] = temp;
                        }

                    }
                }
            }
            return View(calendarprices);

        }
    }
}