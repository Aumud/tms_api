namespace TmsApi.Application.DTOs;
public record CourseResponseDto(
int Id,
string Code,
string Title,
int MaxCapacity,
int EnrollmentCount)

{
    public IReadOnlyList<LinkDto> Links { get; init; } = [];
}