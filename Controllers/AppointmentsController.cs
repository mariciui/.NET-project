using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tema2.Data;
using Tema2.Models;
using Tema2.Services;

namespace Tema2.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private AppointmentService appService;
        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
            appService = new AppointmentService(context);
        }

        // GET: Appointments
        public ViewResult Index()
        {
       
            return View(appService.Index().ToList());
        }

        // GET: Appointments/Details/5
        public ViewResult Details(int id)
        {

            Appointment appointment = appService.Details(id);

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Date,Client,Phone,Car,Problem,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appService.Create(appointment);
                appService.Save();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = appService.getByID(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Date,Client,Phone,Car,Problem, Status")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appService.Edit(appointment);
                    appService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = appService.getByID(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appService.Delete(id);
            appService.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return (appService.getByID(id) != null);
        }

        public ActionResult Check(int id)
        {
            appService.checkAppointment(id);
           appService.Save();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult SearchClient(string Client)
        {
            var toReturn = appService.showAppointments(Client);
            return View(toReturn.ToList());
        }

        public ActionResult SearchByDate(DateTime date)
        {
            var app = appService.Index();
            var toReturn = appService.showAppointments(date);
            return View(toReturn.ToList());
        }

        public async Task<IActionResult> SearchBetweenDates(DateTime start, DateTime stop)
        { 
            var toReturn = appService.SearchBetweenDates(start,stop);
            return View(toReturn.ToList());
        }

        public ActionResult Export(int id)
        {

            if ( id == 1)
            {
                appService.exportReportJSON(appService.GetAllAppointments(),"Json1");
            }

            if (id == 2)
            {
                appService.exportReportCSV(appService.GetAllAppointments(), "CSV.csv");
            }
            return RedirectToAction(nameof(Index));
        }

    }
}

