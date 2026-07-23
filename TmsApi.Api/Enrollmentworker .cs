using TmsApi.Services;

public class EnrollmentWorker(IServiceScopeFactory scopeFactory)
{
    public void ProcessBatch()
    {
        using var scope = scopeFactory.CreateScope();
        var svc = scope.ServiceProvider.GetRequiredService<IEnrollmentService>();

        // TODO 4: Use the service — the 'using' block above disposes it automatically.
        // Example: simulate recalculating scholarships for any enrolled students.
        // svc.EnrollAsync("STU001", "CS101").GetAwaiter().GetResult();

        // (Real logic would go here — for now we just resolve cleanly.)
     // suppress unused-variable warning
    }
}