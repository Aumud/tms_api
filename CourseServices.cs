using TmsApi.Entities;
// --- The contract ---
public interface ICourseService
{
    Task<Course> CreateAsync(string code, string title, int capacity);
    Task<Course?> GetByCodeAsync(string code);
    Task<IReadOnlyList<Course>> GetAllAsync();
    Task<bool> DeleteAsync(string code);
}

// --- The in-memory implementation ---
public class CourseService : ICourseService
{
    private readonly Dictionary<string, Course> _store = new();
    private readonly ILogger<CourseService> _logger;

    public CourseService(ILogger<CourseService> logger)
    {
        _logger = logger;
    }

    public Task<Course> CreateAsync(string code, string title, int capacity)
    {
        if (_store.ContainsKey(code))
        {
            _logger.LogWarning("Course {Code} already exists", code);
            throw new InvalidOperationException($"Course with code {code} already exists");
        }

        var course = new Course
            {
                Code = code,
                Title = title,
                Capacity = capacity
            };
        _store[code] = course;

        _logger.LogInformation("Created course {Code} {Title}", code, title);
        return Task.FromResult(course);
    }

    public Task<Course?> GetByCodeAsync(string code)
    {
        _store.TryGetValue(code, out var course);

        if (course is null)
        {
            _logger.LogWarning("Course {Code} not found", code);
        }

        return Task.FromResult(course);
    }

    public Task<IReadOnlyList<Course>> GetAllAsync()
    {
        IReadOnlyList<Course> all = _store.Values.ToList();
        return Task.FromResult(all);
    }

    public Task<bool> DeleteAsync(string code)
    {
        var removed = _store.Remove(code);

        if (removed)
            _logger.LogInformation("Deleted course {Code}", code);
        else
            _logger.LogWarning("Delete failed course {Code} not found", code);

        return Task.FromResult(removed);
    }
}

// --- The data shape ---
// public record Course(
//     string Code,
//     string Title,
//     int Capacity);

public record CreateCourseRequest(
    string Code,
    string Title,
    int Capacity);
