// --- The contract ---
using TmsApi.Entities;
public interface IStudentService
{
    Task<Student> CreateAsync(string id, string name, int age, decimal gpa);
    Task<Student?> GetByIdAsync(string id);
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
}

// --- The in-memory implementation ---
public class StudentService : IStudentService
{
    private readonly Dictionary<string, Student> _store = new();
    private readonly ILogger<StudentService> _logger;

    public StudentService(ILogger<StudentService> logger)
    {
        _logger = logger;
    }

    public Task<Student> CreateAsync(string id, string name, int age, decimal gpa)
    {
        if (_store.ContainsKey(id))
        {
            _logger.LogWarning("Student {Id} already exists", id);
            throw new InvalidOperationException($"Student with id {id} already exists");
        }

        var student = new Student
            {
                RegistrationNumber = id, // 'id' is actually the registration number
                Name = name,
                Age = age,
                GPA = gpa
            };
        _store[id] = student;

        _logger.LogInformation("Created student {Id} {Name}", id, name);
        return Task.FromResult(student);
    }

    public Task<Student?> GetByIdAsync(string id)
    {
        _store.TryGetValue(id, out var student);

        if (student is null)
        {
            _logger.LogWarning("Student {Id} not found", id);
        }

        return Task.FromResult(student);
    }

    public Task<IReadOnlyList<Student>> GetAllAsync()
    {
        IReadOnlyList<Student> all = _store.Values.ToList();
        return Task.FromResult(all);
    }

    public Task<bool> DeleteAsync(string id)
    {
        var removed = _store.Remove(id);

        if (removed)
            _logger.LogInformation("Deleted student {Id}", id);
        else
            _logger.LogWarning("Delete failed student {Id} not found", id);

        return Task.FromResult(removed);
    }
}

// --- The data shape ---
// public record Student(
//     string Id,
//     string Name,
//     int Age,
//     decimal GPA);

public record CreateStudentRequest(
    string Id,
    string Name,
    int Age,
    decimal GPA);
