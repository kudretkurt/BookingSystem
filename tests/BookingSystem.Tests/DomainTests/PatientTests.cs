using BookingSystem.Domain.Patients;
using BookingSystem.Domain.Psychologists;
using FluentAssertions;

namespace BookingSystem.Tests.DomainTests;

public class PatientTests
{
    [Fact]
    public void IsNotAllowedToConnectMoreThan2_WhenPatientHas2Psychologist_Returns_MaxConnectedPsychologistError()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        patient.ConnectPsychologist(Psychologist.Create("Doctor1", "Doctor1_LastName").Value);
        patient.ConnectPsychologist(Psychologist.Create("Doctor2", "Doctor2_LastName").Value);

        //Act
        var result = patient.ConnectPsychologist(Psychologist.Create("Doctor3", "Doctor3_LastName").Value);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.MaxConnectedPsychologist);
    }

    [Fact]
    public void AlreadyConnected_WhenPatientHasAlreadyConnectionWithPsychologist_Returns_MaxConnectedPsychologistError()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;
        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var result = patient.ConnectPsychologist(psychologist);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.AlreadyConnectedPsychologist);
    }

    [Fact]
    public void CreateAppointment_WhenEverythingIsOk_ReturnsTrue()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var result = patient.CreateAppointment(psychologist.Id, DateOnly.FromDateTime(DateTime.UtcNow), startTime,
            endTime);

        //Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Error.Code.Should().BeEmpty();
        result.Error.Description.Should().BeEmpty();
    }

    [Fact]
    public void
        CanNotCreateAppointment_WhenPsychologistWereNotConnectedWithPatient_Returns_CreateAppointmentJustWithConnectedPsychologist()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;


        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var result =
            patient.CreateAppointment(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow), startTime, endTime);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.CreateAppointmentJustWithConnectedPsychologist);
    }

    [Fact]
    public void CanNotCreateAppointment_WhenAppointmentRequestIsNotProper_Returns_NotConvenientAppointmentRequest()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var result = patient.CreateAppointment(psychologist.Id, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            startTime, endTime);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.NotConvenientAppointmentRequest(
            $"{psychologist.FirstName} {psychologist.LastName}", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            startTime, endTime));
    }

    [Fact]
    public void CanNotCreateAppointment_WhenYouHaveConflictWithAnotherAppointment_Returns_ConflictAppointment()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime.AddMinutes(29), endTime.AddMinutes(29));

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        //First
        patient.CreateAppointment(psychologist.Id, DateOnly.FromDateTime(DateTime.UtcNow), startTime, endTime);
        //Second
        var secondAppointmentResult = patient.CreateAppointment(psychologist.Id,
            DateOnly.FromDateTime(DateTime.UtcNow), startTime.AddMinutes(29), endTime.AddMinutes(29));


        //Assert
        secondAppointmentResult.IsFailure.Should().BeTrue();
        secondAppointmentResult.Error.Should()
            .Be(PatientErrors.ConflictAppointment(DateOnly.FromDateTime(DateTime.UtcNow), startTime.AddMinutes(29),
                endTime.AddMinutes(29)));
    }

    [Fact]
    public void CancelAppointment_WhenEverythingIsOk_Returns_True()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        var dateTo = dateFrom;
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);
        var xx = psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        patient.ConnectPsychologist(psychologist);

        //Act
        var appointmentResult = patient.CreateAppointment(psychologist.Id, dateFrom, startTime, endTime);
        var result = patient.CancelAppointment(patient.Appointments.First().Id);

        //Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Error.Code.Should().BeEmpty();
        result.Error.Description.Should().BeEmpty();
    }

    [Fact]
    public void CanNotCancelAppointment_WhenAppointmentDoesNotExist_Returns_AppointmentDoesNotExist()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(10));
        var dateTo = dateFrom;
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var result = patient.CancelAppointment(Guid.NewGuid());

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.AppointmentDoesNotExist);
    }

    [Fact]
    public void CanNotCancelAppointment_WhenLessThan1HourLeftToStartAppointment_Returns_Availability1HourRule()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = dateFrom;
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddMinutes(5);
        var endTime = startTime.AddMinutes(30);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var appointmentResult = patient.CreateAppointment(psychologist.Id, dateFrom, startTime, endTime);
        var result = patient.CancelAppointment(patient.Appointments.First().Id);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.Availability1HourRule);
    }

    [Fact]
    public void CanNotCancelAppointment_WhenLessThan1HourLeftToStartAppointment_Returns_CanNotDeletePassedAppointment()
    {
        //Arrange
        var patientResult = Patient.Create("Kudret", "Kurt", 33);
        var patient = patientResult.Value;
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1_LastName");
        var psychologist = psychologistResult.Value;

        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = dateFrom;
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddMinutes(-5);
        var endTime = startTime.AddMinutes(30);
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        patient.ConnectPsychologist(psychologist);
        //patient.ConnectedPsychologists.Add(psychologist);

        //Act
        var appointmentResult = patient.CreateAppointment(psychologist.Id, dateFrom, startTime, endTime);
        var result = patient.CancelAppointment(patient.Appointments.First().Id);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PatientErrors.CanNotDeletePassedAppointment);
    }
}