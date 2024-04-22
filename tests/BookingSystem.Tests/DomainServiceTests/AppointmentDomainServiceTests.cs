using BookingSystem.Domain.Patients;
using BookingSystem.Domain.Psychologists;
using FluentAssertions;

namespace BookingSystem.Tests.DomainServiceTests;

public class AppointmentDomainServiceTests(RepositoryFixture fixture) : IClassFixture<RepositoryFixture>
{
    [Fact]
    public async Task CreateAppointment_WhenEverythingIsOk_ReturnsTrue()
    {
        //Arrange
        var patientResult = Patient.Create("kudret", "kurt", 33);
        var patient = patientResult.Value;
        await fixture.PatientRepository.Insert(patient, new CancellationToken(false));

        var psychologistResult = Psychologist.Create("doctorFirstName", "doctorLastName");
        var psychologist = psychologistResult.Value;
        await fixture.PsychologistRepository.Insert(psychologist, new CancellationToken(false));

        var patientConnectionResult = patient.ConnectPsychologist(psychologist);
        await fixture.PatientRepository.UpdateAsync(patient, new CancellationToken(false));

        var date = DateOnly.ParseExact("2024-04-22", "yyyy-MM-dd");
        var startTime = TimeOnly.ParseExact("11:00:00", "HH:mm:ss");
        var endTime = TimeOnly.ParseExact("11:30:00", "HH:mm:ss");

        var availabilityResult = psychologist.CreateAvailability(date, date, startTime, endTime);
        await fixture.PsychologistRepository.UpdateAsync(psychologist, new CancellationToken(false));

        //Act
        var appointmentResult = await fixture.AppointmentDomainService.CreateAppointment(patient.Id, psychologist.Id,
            date,
            startTime, endTime, new CancellationToken(false));

        //Assert
        appointmentResult.IsSuccess.Should().BeTrue();
        var appointment = await
            fixture.AppointmentRepository.GetAppointmentByIdAsync(appointmentResult.Value,
                new CancellationToken(false));

        appointment.Should().NotBeNull();
        appointment!.PatientId.Should().Be(patient.Id);
        appointment!.PsychologistId.Should().Be(psychologist.Id);
    }

    [Fact]
    public async Task CancelAppointment_WhenEverythingIsOk_ReturnsTrue()
    {
        //Arrange
        var patientResult = Patient.Create("patient1FirstName", "patient1LastName", 33);
        var patient = patientResult.Value;
        await fixture.PatientRepository.Insert(patient, new CancellationToken(false));

        var psychologistResult = Psychologist.Create("doctorFirstName", "doctorLastName");
        var psychologist = psychologistResult.Value;
        await fixture.PsychologistRepository.Insert(psychologist, new CancellationToken(false));

        var patientConnectionResult = patient.ConnectPsychologist(psychologist);
        await fixture.PatientRepository.UpdateAsync(patient, new CancellationToken(false));

        var date = DateOnly.ParseExact("2024-04-22", "yyyy-MM-dd");
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(10);
        var endTime = startTime.AddMinutes(30);

        var availabilityResult = psychologist.CreateAvailability(date, date, startTime, endTime);
        await fixture.PsychologistRepository.UpdateAsync(psychologist, new CancellationToken(false));

        var appointmentResult = await fixture.AppointmentDomainService.CreateAppointment(patient.Id, psychologist.Id,
            date,
            startTime, endTime, new CancellationToken(false));

        //Act
        var cancelAppointmentResult =
            await fixture.AppointmentDomainService.CancelAppointment(appointmentResult.Value,
                new CancellationToken(false));


        //Assert
        cancelAppointmentResult.IsSuccess.Should().BeTrue();

        var appointment = await
            fixture.AppointmentRepository.GetAppointmentByIdAsync(appointmentResult.Value,
                new CancellationToken(false));

        appointment.Should().BeNull();
    }
}