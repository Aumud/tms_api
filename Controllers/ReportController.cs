using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Entities;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController(TmsDbContext context) : ControllerBase
{
        [HttpGet("active-students")]
    public async Task<IActionResult> ActiveStudents()
    {
        var count = await context.Students
            .Where(s => s.IsActive && s.GPA >= 3.0m)
            .CountAsync();

        return Ok(new { Count = count });
    }

        [HttpGet("course-enrollments")]
    public async Task<IActionResult> CourseEnrollments()
    {
        var list = await context.Courses
            .Select(c => new
            {
                c.Title,
                EnrollmentCount = c.Enrollments.Count
            })
            .OrderByDescending(x => x.EnrollmentCount)
                .Take(5)
                .ToListAsync();

        return Ok(list);
    }

        [HttpGet("average-gpa")]
    public async Task<IActionResult> AverageGpa()
    {
        var list = await context.Enrollments
            .GroupBy(e => e.Course.Title)
            .Select(g => new
            {
                Course = g.Key,
                AverageGPA = g.Average(e => e.Student.GPA)
            })
            .ToListAsync();

        return Ok(list);
    }

        [HttpGet("no-enrollments-subquery")]
    public async Task<IActionResult> NoEnrollmentsSubquery()
    {
        var list = await context.Students
            .Where(s => !s.Enrollments.Any())
            .Select(s => s.Name)
            .ToListAsync();

        return Ok(list);
    }

        [HttpGet("no-enrollments-leftjoin")]
    public async Task<IActionResult> NoEnrollmentsLeftJoin()
    {
        var list = await context.Students
            .LeftJoin(
                context.Enrollments,
                s => s.Id,
                e => e.StudentId,
                (s, e) => new { s, e })
            .Where(x => x.e == null)
            .Select(x => x.s.Name)
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("students")]
        public async Task<IActionResult> GetStudents(int page = 1)
        {
            const int pageSize = 20;

            var students = await context.Students
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(students);
        }

    [HttpGet("nplusone")]
        public async Task<IActionResult> NPlusOne()
        {
            var students = await context.Students
                .AsNoTracking()
                .ToListAsync();

            var result = new List<object>();

            foreach (var s in students)
            {
                var count = await context.Enrollments
                    .AsNoTracking()
                    .CountAsync(e => e.StudentId == s.Id);

                result.Add(new
                {
                    s.Name,
                    EnrollmentCount = count
                });
            }

            return Ok(result);
        }

    [HttpGet("nplusone-fixed")]
        public async Task<IActionResult> NPlusOneFixed()
        {
            var report = await context.Students
                .AsNoTracking()
                .Select(s => new
                {
                    s.Name,
                    EnrollmentCount = s.Enrollments.Count
                })
                .ToListAsync();

            return Ok(report);
        }

    [HttpDelete("students/{id}")]
        public async Task<IActionResult> SoftDeleteStudent(int id)
        {
            var student = await context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            student.IsDeleted = true;

            await context.SaveChangesAsync();

            return NoContent();
        }



    [HttpGet("students/all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await context.Students
                .IgnoreQueryFilters()
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(students);
        }
    [HttpPost("archive-old-enrollments")]
            public async Task<IActionResult> ArchiveOldEnrollments()
            {
                var rows = await context.Enrollments
                    .Where(e => e.EnrolledAt < DateTime.UtcNow.AddYears(-1))
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(e => e.IsArchived, true));

                return Ok(new
                {
                    UpdatedRows = rows
                });
            }

}