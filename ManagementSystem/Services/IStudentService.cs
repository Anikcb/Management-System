using ManagementSystem.Models;

namespace ManagementSystem.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAsync();
        Task<Student?> GetAsync(string id);
        Task<List<Student>> GetAsync(int index);
        Task CreateAsync(Student newStudent);
        Task UpdateAsync(string id, Student student);
        Task RemoveAsync(string id);
    }
}
