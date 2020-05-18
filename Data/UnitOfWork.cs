using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tema2.Models;

namespace Tema2.Data
{
    public class UnitOfWork : IDisposable
    {
        private  ApplicationDbContext context = new ApplicationDbContext();
        private  AppointmentRepository appointmentRepository;
    
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }
        public AppointmentRepository AppointmentRepository
        {
            get
            {

                if (this.appointmentRepository == null)
                {
                    this.appointmentRepository = new AppointmentRepository(context);
                }
                return appointmentRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

