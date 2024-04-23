using Asp.Versioning;
using BookingSystem.API.Endpoints;
using BookingSystem.API.Infrastructure;
using BookingSystem.API.Installers;
using BookingSystem.Application;
using BookingSystem.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

ConfigurationSettings(builder.Configuration);

//I added rateLimiter to protect our API for unexpected request.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(10);
    });
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.InstallSwagger(builder.Environment);

builder.Services.AddOutputCache(builder => builder.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

//I added for gracefully shutdown
builder.Services.AddDelayedShutdownHostLifeTime();

var app = builder.Build();

ConfigureGracefullyShutdown(app);

MapEndpoints();

void ConfigureGracefullyShutdown(WebApplication app)
{
    //PS : I added two different code piece for gracefully shutdown , because we have 2 different topic . 
    //First one is that When we receive SIGTERM signal from kubernetes or docker (Thats why I added this line --> builder.Services.AddDelayedShutdownHostLifeTime();)
    //second one is that when applicationStopping event raised how can we linked request's cancellationToken with that
    //For the second one , explanation is below ; 
    //https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/graceful-shutdown/graceful-shutdown.md
    //This CancellationToken is automatically created by ASP.NET and will be activated when the request is cancelled by the client.
    //By default, the same CancellationToken will NOT be activated when ApplicationStopping event is raised, but we can create a "linked" CancellationToken to cover both cases
    app.Use((httpContext, next) =>
    {
        var hostLifetime = httpContext.RequestServices.GetRequiredService<IHostApplicationLifetime>();
        var originalCancellationToken = httpContext.RequestAborted;
        var combinedCt = CancellationTokenSource
            .CreateLinkedTokenSource(originalCancellationToken, hostLifetime.ApplicationStopping).Token;
        httpContext.RequestAborted = combinedCt;
        return next(httpContext);
    });
}

void ConfigurationSettings(IConfigurationBuilder configurationBuilder)
{
    configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
    configurationBuilder.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
    configurationBuilder.AddEnvironmentVariables();
}

void MapEndpoints()
{
    var apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(0))
        .HasApiVersion(new ApiVersion(1))
        .ReportApiVersions()
        .Build();

    var versionedGroup = app
        .MapGroup("api/v{apiVersion:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

    versionedGroup.MapPatientEndpoints();
    versionedGroup.MapPsychologistEndpoints();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            // build a swagger endpoint for each discovered API version
            foreach (var description in app.DescribeApiVersions())
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
        });
}

app.UseHttpsRedirection();

app.UseOutputCache();

app.MapHealthChecks("/api/health");

app.UseRateLimiter();

app.UseExceptionHandler();

app.Run();