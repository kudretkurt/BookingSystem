using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Psychologists;
using FluentAssertions;

namespace BookingSystem.Tests.DomainTests;

public class PsychologistTests
{
    [Fact]
    public void CreateAvailability_WhenEverythingIsOk_ReturnsTrue()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);

        //Act
        var result = psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        //Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CanNotCreateAvailability_WhenAvailabilityIsNotUnique_ReturnsTrue()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);

        //Act
        var firstAvailabilityResult = psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);
        var secondAvailabilityResult = psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);

        //Assert
        secondAvailabilityResult.IsFailure.Should().BeTrue();
        secondAvailabilityResult.Error.Should()
            .Be(PsychologistErrors.AvailabilityNotUnique(dateFrom, startTime, endTime));
    }

    [Fact]
    public void CanEditAvailability_WhenEverythingIsOk_ReturnsTrue()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);

        //Act
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);
        var currentAvailability = psychologist.Availabilities.First();
        var result = psychologist.EditAvailability(currentAvailability.Date, currentAvailability.StartTime,
            currentAvailability.EndTime, dateFrom, startTime.AddMinutes(30), endTime.AddMinutes(30));

        //Assert
        result.IsSuccess.Should().BeTrue();
        psychologist.Availabilities.Count.Should().Be(1);
        psychologist.Availabilities.First().Should().NotBeNull();
        psychologist.Availabilities.First().StartTime.Should().Be(startTime.AddMinutes(30));
        psychologist.Availabilities.First().EndTime.Should().Be(endTime.AddMinutes(30));
    }

    [Fact]
    public void CanNotEditAvailability_WhenAvailabilityDoesNotExist_ReturnsTrue()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateTo = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var endTime = startTime.AddMinutes(30);

        //Act
        psychologist.CreateAvailability(dateFrom, dateTo, startTime, endTime);
        var result =
            psychologist.EditAvailability(dateFrom.AddDays(1), startTime, endTime, dateFrom, startTime.AddMinutes(30),
                endTime.AddMinutes(30));

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should()
            .Be(PsychologistErrors.AvailabilityNotFound(dateFrom.AddDays(1), startTime,
                endTime));
    }

    [Fact]
    public void CancelAppointment_WhenEverythingIsOk_ReturnsTrue()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(2);
        var endTime = startTime.AddMinutes(30);
        var appointmentResult = Appointment.Create(psychologist.Id,
            Guid.NewGuid(), dateFrom, startTime, endTime);
        psychologist.Appointments.Add(appointmentResult.Value);

        //Act
        var result = psychologist.CancelAppointment(appointmentResult.Value.Id);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CanNotCancelAppointment_WhenLessThan1HourLeftToStartAppointment_Returns_Availability1HourRule()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddMinutes(30));
        var endTime = startTime.AddMinutes(30);
        var appointmentResult = Appointment.Create(psychologist.Id,
            Guid.NewGuid(), dateFrom, startTime, endTime);
        psychologist.Appointments.Add(appointmentResult.Value);

        //Act
        var result = psychologist.CancelAppointment(appointmentResult.Value.Id);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PsychologistErrors.Availability1HourRule);
    }

    [Fact]
    public void CanNotCancelAppointment_WhenItAlreadyPassed_Returns_CanNotDeletePassedAppointment()
    {
        //Arrange
        var psychologistResult = Psychologist.Create("Doctor1", "Doctor1LastName");
        var psychologist = psychologistResult.Value;
        var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow);
        var startTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddMinutes(-30));
        var endTime = startTime.AddMinutes(30);
        var appointmentResult = Appointment.Create(psychologist.Id,
            Guid.NewGuid(), dateFrom, startTime, endTime);
        psychologist.Appointments.Add(appointmentResult.Value);

        //Act
        var result = psychologist.CancelAppointment(appointmentResult.Value.Id);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PsychologistErrors.CanNotDeletePassedAppointment);
    }
}