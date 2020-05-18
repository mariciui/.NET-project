using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.HttpSys;
using Tema2.Data;
using Tema2.Models;

namespace Tema2.Services
{
    public class AppointmentService
    {
       private UnitOfWork unitOfWork;
        private Stack<Report> reportStack = new Stack<Report>();

        public AppointmentService(ApplicationDbContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }
        public List<Appointment> GetAllAppointments()
        {
            return unitOfWork.AppointmentRepository.GetAllAppointments();
        }
        public List<Appointment> Index()
        {
            return (unitOfWork.AppointmentRepository.Get()).ToList();
        }

        public Appointment Details(int id)
        {
            return unitOfWork.AppointmentRepository.GetByID(id);
        }

        public void Create(Appointment appointment)
        {
            unitOfWork.AppointmentRepository.Insert(appointment);
        }

        public void Edit(Appointment appointment)
        {
           unitOfWork.AppointmentRepository.Update(appointment);
        }

        public void Delete(int ID)
        {
            unitOfWork.AppointmentRepository.Delete(ID);
        }
        public void Save()
        {
            unitOfWork.AppointmentRepository.Save();
        }

        public Appointment getByID(int id)
        {
            return unitOfWork.AppointmentRepository.GetByID(id);
        }

        public List<Appointment> showAppointments(DateTime date)
        {
            var appointments = unitOfWork.AppointmentRepository.Get();
            List<Appointment> toReturn = new List<Appointment>(); 
            foreach (var a in appointments)
            {
                if (a.Date.Date.Equals(date.Date))
                    toReturn.Add(a);
            }

            return toReturn;
        }

        public List<Appointment> SearchBetweenDates(DateTime start, DateTime stop)
        {
            List<Appointment> toReturn = new List<Appointment>();
            var appointments = unitOfWork.AppointmentRepository.Get();

            foreach (var a in appointments)
            {
                if (a.Date.Date > start.Date && a.Date.Date < stop.Date )
                    toReturn.Add(a);
            }

            return toReturn;
        }
        public void checkAppointment(int ID)
        {

            Appointment appointment = unitOfWork.AppointmentRepository.GetByID(ID);

            appointment.Status = "YES";
            //unitOfWork.AppointmentRepository.Save();

            unitOfWork.AppointmentRepository.Update(appointment);

        }


        public List<Appointment> showAppointments(string Client)
        {
            var appointments = unitOfWork.AppointmentRepository.Get();
            List<Appointment> toReturn = new List<Appointment>();
            foreach (var a in appointments)
            {
                if (a.Client.Equals(Client))
                    toReturn.Add(a);
            }

            return toReturn;
        }

        public void exportReportJSON(List<Appointment> list, string path)
        {
            Report r = ReportFactory.Create(ReportFactory.ReportTypes.Json);
            reportStack.Push(r);
            r.Export(list, path);
        }
        public void exportReportCSV(List<Appointment> list, string path)
        {
            Report r = ReportFactory.Create(ReportFactory.ReportTypes.CSV);
            reportStack.Push(r);
            r.Export(list, path);
        }


    }
}
