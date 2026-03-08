using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingController : ControllerBase
    {
        // GET api/borrowing
        [HttpGet]
        public IActionResult GetAllBorrowings()
        {
            return Ok("Returns a list of all borrowings.");
        }

        // GET api/borrowing/{id}
        [HttpGet("{id}")]
        public IActionResult GetBorrowingById(int id)
        {
            return Ok($"Returns the borrowing record with ID {id}.");
        }

        // POST api/borrowing
        [HttpPost]
        public IActionResult CreateBorrowing([FromBody] Borrowing borrowing)
        {
            return CreatedAtAction(nameof(GetBorrowingById), new { id = 1 }, "Borrowing record created successfully.");
        }

        // PUT api/borrowing/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBorrowing(int id, [FromBody] Borrowing borrowing)
        {
            return Ok($"Borrowing record with ID {id} updated successfully.");
        }

        // DELETE api/borrowing/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBorrowing(int id)
        {
            return Ok($"Borrowing record with ID {id} deleted successfully.");
        }
    }
}
