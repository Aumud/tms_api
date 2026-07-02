using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;
namespace TmsApi.Data;
public class TmsDbContext(DbContextOptions<TmsDbContext> options) : DbContext(options)
{
    public DbSet<TmsApi.Entities.Course> Courses => Set<TmsApi.Entities.Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<Certificate> Certificates =>Set<Certificate>();
}