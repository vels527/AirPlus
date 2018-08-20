using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreAirPlus.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using CoreAirPlus.Entities;
using CoreAirPlus.ViewModel;
using System;

namespace CoreAirPlus.Controllers
{
    public class HomeController : Controller
    {
        private IReadRepository _readRepository;
        public HomeController(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }
        // GET: Home
        public ActionResult Index()
        {
            int? CurrentThisHostId = HttpContext.Session.GetInt32("HostId");
            int hostid = CurrentThisHostId == null ? -1 : CurrentThisHostId.Value;
            if (hostid == -1)
            {
                throw new System.Exception("Session Host not Set");
            }
            var reservations = _readRepository.GetReservationsByHost(hostid);
            var reservationViewModel = reservations.Select(c => new ReservationViewModel {
                GuestName = c.guest.FullName,
                CheckIn = c.CheckIn,
                CheckOut = c.CheckOut,
                RCheckIn = c.RCheckIn == null ? DateTime.MinValue.TimeOfDay : c.RCheckIn.Value.TimeOfDay,
                RCheckOut = c.RCheckOut == null ? DateTime.MinValue.TimeOfDay : c.RCheckOut.Value.TimeOfDay,
                Remarks = c.Remarks,
                CleaningTime=c.CleaningTime,
                Status=c.status
            });
            return View(reservationViewModel);
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
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