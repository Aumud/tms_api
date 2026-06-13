//using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddScoped<EnrollmentWorker>();
builder.Services.AddSingleton<IEnrollmentService, EnrollmentService>();


builder.Services.AddControllers();

builder.Host.UseDefaultServiceProvider(options =>
{
options.ValidateScopes = true;
options.ValidateOnBuild = true;
});






var app = builder.Build();
// Configure the HTTP request pipeline.

app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
{
worker.ProcessBatch();
return Results.Ok("processed");
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGet("/api/error", () =>
{
throw new TmsDatabaseException("Simulated database failure for ProblemDetails testing");
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseExceptionHandler(); 
}


app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();
 app.UseStatusCodePages();


app.Run();

