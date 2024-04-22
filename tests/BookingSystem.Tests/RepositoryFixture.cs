using BookingSystem.Application;
using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Patients;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Infrastructure.Data;
using BookingSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Tests;

public class RepositoryFixture
{
    private readonly ServiceProvider _serviceProvider;

    public RepositoryFixture()
    {
        var sc = new ServiceCollection();

        sc.AddDbContext<ApplicationDbContext>(options =>
            //enable this if you want an in-memoryDB
            options.UseInMemoryDatabase("inmemoryDBInstance"));

        sc.AddApplication();

        sc.AddScoped<IPatientRepository, PatientRepository>();
        sc.AddScoped<IPsychologistRepository, PsychologistRepository>();
        sc.AddScoped<IAppointmentRepository, AppointmentRepository>();

        _serviceProvider = sc.BuildServiceProvider();
    }

    public IPatientRepository PatientRepository => _serviceProvider.GetRequiredService<IPatientRepository>();

    public IPsychologistRepository PsychologistRepository =>
        _serviceProvider.GetRequiredService<IPsychologistRepository>();

    public IAppointmentRepository AppointmentRepository =>
        _serviceProvider.GetRequiredService<IAppointmentRepository>();

    public IAppointmentDomainService AppointmentDomainService =>
        _serviceProvider.GetRequiredService<IAppointmentDomainService>();
}