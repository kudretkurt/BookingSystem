using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Patients;
using BookingSystem.Domain.Psychologists;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }


    public DbSet<Patient> Patients { get; set; }
    public DbSet<Psychologist> Psychologists { get; set; }
    public DbSet<Appointment> Appointments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientConnection>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Patient>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Appointment>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Psychologist>()
            .HasKey(p => p.Id);

        //To prevent duplicate appointment record for different patient but same psychologist and same TimeSlot
        modelBuilder.Entity<Appointment>()
            .HasIndex(p => new { p.PsychologistId, p.Date, p.StartTime, p.EndTime })
            .IsUnique();

        //To prevent appointment record which for same patient but with different psychologist and same Time Slot
        modelBuilder.Entity<Appointment>()
            .HasIndex(p => new { p.PatientId, p.Date, p.StartTime, p.EndTime })
            .IsUnique();

        modelBuilder.Entity<Appointment>()
            .HasOne<Patient>()
            .WithMany(t => t.Appointments)
            .HasForeignKey(t => t.PatientId);

        modelBuilder.Entity<Appointment>()
            .HasOne<Psychologist>()
            .WithMany(t => t.Appointments)
            .HasForeignKey(t => t.PsychologistId);

        modelBuilder.Entity<Patient>()
            .HasMany(t => t.ConnectedPsychologists)
            .WithOne();

        modelBuilder.Entity<Psychologist>()
            .OwnsMany(t => t.Availabilities);
    }
}