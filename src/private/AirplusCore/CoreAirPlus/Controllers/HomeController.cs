using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreAirPlus.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using CoreAirPlus.Entities;
using CoreAirPlus.ViewModel;
using System;
using System.Collections.Generic;

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

        // POST: Home/Edit/5
        [HttpPost("Edit")]
        public ActionResult Edit(ReservationViewModel reservation)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {

                }
                else
                {
                    //ModelState.AddModelError()
                    return View(reservation);
                }

                return RedirectToAction("Index");
            }
            catch
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
    }
}