using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using TmsApi.Dtos;
using TmsApi.Services;
namespace Tms.Api.Controllers;
[ApiController]
[Route("api/courses/{courseId:int}/enrollments")]
public class EnrollmentsController(
ICourseService courseService,
IEnrollmentService enrollmentService) : ControllerBase
{
[HttpGet("{id:int}", Name = nameof(GetEnrollment))]
public async Task<IActionResult> GetEnrollment(int courseId, int id,
CancellationToken ct)
{
var enrollment = await enrollmentService.GetByIdAsync(courseId,
id, ct);
return enrollment is not null ? Ok(enrollment) : NotFound();
}
[HttpPost]
public async Task<IActionResult> EnrollStudent(int courseId, EnrollStudentRequest request, CancellationToken ct)
{
// TODO 3: Look up the parent course (courseService.GetByIdAsync). If null, return NotFound().
    var course = await courseService.GetByIdAsync(courseId, ct);
    if(course is null)
        {
            return NotFound();
        }
// Then check capacity (course.EnrollmentCount >= course.MaxCapacity).
// If full, return Conflict(new ProblemDetails { ... })with:
// Title = "Course is full"
// Detail = $"Course '{course.Title}' has reached itsmaximum capacity of {course.MaxCapacity}."
// Status = StatusCodes.Status409Conflict
    if(course.EnrollmentCount >= course.MaxCapacity)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Code is full",
                Detail = $"Course '{course.Title}' has reached its maximumcapacity of {course.MaxCapacity}.",
                Status = StatusCodes.Status409Conflict
            });
        }
// Otherwise, call enrollmentService.CreateAsync and return CreatedAtAction(nameof(GetEnrollment),
// new { courseId, id = enrollment.Id }, enrollment).
    var enrollment = await enrollmentService.CreateAsync(courseId, request, ct);
    return CreatedAtAction(nameof(GetEnrollment), new{courseId, id = enrollment.Id}, enrollment);
throw new NotImplementedException();
}
}