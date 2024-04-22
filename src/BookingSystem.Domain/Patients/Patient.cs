using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;

namespace BookingSystem.Domain.Patients;

public sealed class Patient : Entity
{
    private Patient(Guid id, string firstName, string lastName, int age) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Appointments = new List<Appointment>();
        ConnectedPsychologists = new List<PatientConnection>();
    }

    private Patient()
    {
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }
    public List<Appointment> Appointments { get; }
    public List<PatientConnection> ConnectedPsychologists { get; }

    public static Result<Patient> Create(string firstName, string lastName, int age)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return Result.Failure<Patient>(PatientErrors.EmptyFirstName);

        if (string.IsNullOrWhiteSpace(lastName)) return Result.Failure<Patient>(PatientErrors.EmptyLastName);

        if (age == default) return Result.Failure<Patient>(PatientErrors.EmptyAge);


        return new Patient(Guid.NewGuid(), firstName, lastName, age);
    }

    public Result ConnectPsychologist(Psychologist psychologist)
    {
        //TODO; get count value from config
        if (ConnectedPsychologists.Count == 2) return PatientErrors.MaxConnectedPsychologist;

        if (ConnectedPsychologists.Any(t => t.PsychologistId == psychologist.Id))
            return PatientErrors.AlreadyConnectedPsychologist;

        ConnectedPsychologists.Add(new PatientConnection
            { PsychologistId = psychologist.Id, PatientId = Id, Psychologist = psychologist });

        return Result.Success(this);
    }

    public Result<Appointment> CreateAppointment(Guid psychologistId, DateOnly date, TimeOnly startTime,
        TimeOnly endTime)
    {
        if (ConnectedPsychologists.All(t => t.PsychologistId != psychologistId))
            return Result.Failure<Appointment>(PatientErrors.CreateAppointmentJustWithConnectedPsychologist);

        var patientConnections = ConnectedPsychologists.First(t => t.PsychologistId == psychologistId);

        if (!patientConnections.Psychologist.Availabilities.Any(t =>
                t.Date == date && t.StartTime == startTime && t.EndTime == endTime))
            return Result.Failure<Appointment>(PatientErrors.NotConvenientAppointmentRequest(
                $"{patientConnections.Psychologist.FirstName} {patientConnections.Psychologist.LastName}", date,
                startTime, endTime));

        if (Appointments.Any(t => t.Date == date && t.StartTime <= startTime && t.EndTime > startTime) ||
            Appointments.Any(t => t.Date == date && t.StartTime < endTime && t.EndTime >= endTime))
            return Result.Failure<Appointment>(PatientErrors.ConflictAppointment(date, startTime, endTime));

        var appointmentResult = Appointment.Create(psychologistId, Id, date, startTime, endTime);
        if (appointmentResult.IsFailure) return appointmentResult;
        var appointment = appointmentResult.Value;
        Appointments.Add(appointment);
        return Result.Success(appointment);
    }

    public Result CancelAppointment(Guid appointmentId)
    {
        var appointment = Appointments.FirstOrDefault(t => t.Id == appointmentId);
        if (appointment == null) return PatientErrors.AppointmentDoesNotExist;

        var startTime = appointment.StartTime;
        var appointmentExactDate = appointment.Date.ToDateTime(startTime);

        if (appointmentExactDate < DateTime.UtcNow) return PatientErrors.CanNotDeletePassedAppointment;

        var gap = appointmentExactDate - DateTime.UtcNow;
        if (gap.TotalMinutes <= 60) return PatientErrors.Availability1HourRule;

        Appointments.Remove(appointment);
        return Result.Success(this);
    }
}