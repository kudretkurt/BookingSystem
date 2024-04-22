using BookingSystem.Application.Common;
using BookingSystem.Application.Patient;
using BookingSystem.Application.Psychologist;
using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Patients;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.RegisterMapsterConfiguration();

        services.AddScoped<IAppointmentDomainService, AppointmentDomainService>();

        return services;
    }

    private static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Domain.Patients.Patient, PatientResponse>
            .NewConfig()
            .Map(dest => dest.AvailableSlots, src => src.ConnectedPsychologists)
            .Map(dest => dest.UpComingAppointments, src => src.Appointments);

        TypeAdapterConfig<PatientConnection, AvailableSlot>
            .NewConfig()
            .Map(dest => dest.PsychologistName, src => src.Psychologist.FirstName)
            .Map(dest => dest.PsychologistLastName, src => src.Psychologist.LastName)
            .Map(dest => dest.Availabilities, src => src.Psychologist.Availabilities);

        TypeAdapterConfig<Domain.Appointments.Appointment, UpComingAppointment>
            .NewConfig();

        TypeAdapterConfig<Domain.Psychologists.Psychologist, PsychologistResponse>
            .NewConfig()
            .Map(dest => dest.Availabilities, src => src.Availabilities)
            .Map(dest => dest.UpComingAppointments, src => src.Appointments);
    }
}