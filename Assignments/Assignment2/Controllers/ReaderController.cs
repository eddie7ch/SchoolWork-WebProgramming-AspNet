using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        // GET api/reader
        [HttpGet]
        public IActionResult GetAllReaders()
        {
            return Ok("Returns a list of all readers.");
        }

        // GET api/reader/{id}
        [HttpGet("{id}")]
        public IActionResult GetReaderById(int id)
        {
            return Ok($"Returns the reader with ID {id}.");
        }

        // POST api/reader
        [HttpPost]
        public IActionResult CreateReader([FromBody] Reader reader)
        {
            return CreatedAtAction(nameof(GetReaderById), new { id = 1 }, "Reader created successfully.");
        }

        // PUT api/reader/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateReader(int id, [FromBody] Reader reader)
        {
            return Ok($"Reader with ID {id} updated successfully.");
        }

        // DELETE api/reader/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteReader(int id)
        {
            return Ok($"Reader with ID {id} deleted successfully.");
        }
    }
}
