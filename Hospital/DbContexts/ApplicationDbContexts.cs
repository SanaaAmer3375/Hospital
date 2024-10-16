using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Hospital.DbContexts
{
    public class ApplicationDbContexts : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public ApplicationDbContexts(DbContextOptions options) : base(options)
        {

        }
    }
}