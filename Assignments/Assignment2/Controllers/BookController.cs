using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        // GET api/book
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok("Returns a list of all books.");
        }

        // GET api/book/{id}
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            return Ok($"Returns the book with ID {id}.");
        }

        // POST api/book
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            return CreatedAtAction(nameof(GetBookById), new { id = 1 }, "Book created successfully.");
        }

        // PUT api/book/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            return Ok($"Book with ID {id} updated successfully.");
        }

        // DELETE api/book/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            return Ok($"Book with ID {id} deleted successfully.");
        }
    }
}
