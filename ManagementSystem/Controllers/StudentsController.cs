using ManagementSystem.Models;
using ManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly StudentService _studentService;
        public StudentsController(RedisService redisService, StudentService studentService)
        {
            _redisService = redisService;
            _studentService = studentService;
        }

        // GET: api/<StudentsController>
        [HttpGet]
        public async Task<List<Student>> Get()
        {
            return await _studentService.GetAsync();
        }

        // GET api/<StudentsController>/5
        [HttpGet("{index}")]
        public async Task<List<Student>> Get(int index)
        {
            string id = index.ToString();
            var cachedData = _redisService.GetKey<List<Student>>(id);
            if (cachedData != null)
            {
                return cachedData;
            }
            var mongoData = await _studentService.GetAsync(index*5);
           _redisService.SetKey(id, mongoData);

            return mongoData;
        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student newStudent)
        {
            await _studentService.CreateAsync(newStudent);
            return CreatedAtAction(nameof(Get), new { id = newStudent.Id }, newStudent);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Student updateStudent)
        {
            var student = await _studentService.GetAsync(id);
            if (student is null)
            {
                return NotFound($"Student Id {id} is Missing");
            }
            updateStudent.Id = id;
            await _studentService.UpdateAsync(id, updateStudent);
            return NoContent();
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var student = await _studentService.GetAsync(id);
            if (student is null)
            {
                return NotFound($"Student Id {id} is Missing");
            }
            await _studentService.RemoveAsync(id);
            return NoContent();
        }
    }
}
