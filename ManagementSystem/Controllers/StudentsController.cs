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
        private readonly StudentService _studentService;
        public StudentsController(StudentService studentService)
        {
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
            return await _studentService.GetAsync(index*5);
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
