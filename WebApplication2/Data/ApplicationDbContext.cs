using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");

        }
        public DbSet<Doctor> Doctors { get; set; } //Create table in Database with attributes in Doctor Class
        public DbSet<Patient> Patients { get; set; } //Create table in DB Patient Class
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<UserViewModel> UserViewModel { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
