using ManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ManagementSystem.Services
{
    public class StudentService
    {
        private readonly IMongoCollection<Student> _studentCollection;

        public StudentService(IOptions<StudentStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _studentCollection = mongoDatabase.GetCollection<Student>(
                bookStoreDatabaseSettings.Value.StudentCollectionName);
        }

        public async Task<List<Student>> GetAsync() =>
            await _studentCollection.Find(_ => true).ToListAsync();

        public async Task<Student?> GetAsync(string id) =>
            await _studentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Student>> GetAsync(int index)
        {
            var documents = _studentCollection.Find(FilterDefinition<Student>.Empty).Skip(index-5).Limit(5).ToListAsync();
            return await documents;
        }

        public async Task CreateAsync(Student newStudent) =>
            await _studentCollection.InsertOneAsync(newStudent);

        public async Task UpdateAsync(string id, Student student) =>
            await _studentCollection.ReplaceOneAsync(x => x.Id == id, student);

        public async Task RemoveAsync(string id) =>
            await _studentCollection.DeleteOneAsync(x => x.Id == id);
    }
}
