using ManagementSystem.Models;
using ManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get()
        {
            try
            {
                var students = await _studentService.GetAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Get(): {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<StudentsController>/5
        [HttpGet("{index}")]
        public async Task<IActionResult> Get(int index)
        {
            try
            {
                string id = index.ToString();
                var cachedData = _redisService.GetKey<List<Student>>(id);
                if (cachedData != null)
                {
                    return Ok(cachedData);
                }

                var mongoData = await _studentService.GetAsync(index * 5);
                if(mongoData.Count == 0)
                {
                    return NotFound($"Student List is Missing");
                }

                _redisService.SetKey(id, mongoData);
                return Ok(mongoData);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Get(int index): {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student newStudent)
        {
            try
            {
                await _studentService.CreateAsync(newStudent);
                return CreatedAtAction(nameof(Get), new { id = newStudent.Id }, newStudent);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Post(): {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Student updateStudent)
        {
            try
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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Put(string id): {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var student = await _studentService.GetAsync(id);
                if (student is null)
                {
                    return NotFound($"Student Id {id} is Missing");
                }

                await _studentService.RemoveAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Delete(string id): {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
