using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingController : ControllerBase
    {
        private static List<Borrowing> _borrowings = new List<Borrowing>
        {
            new Borrowing { Id = 1, BookId = 1, ReaderId = 1, BorrowDate = new DateTime(2026, 2, 1), DueDate = new DateTime(2026, 2, 15), ReturnDate = new DateTime(2026, 2, 14), IsReturned = true, OverdueCharge = 0, Notes = "Returned on time." },
            new Borrowing { Id = 2, BookId = 3, ReaderId = 2, BorrowDate = new DateTime(2026, 2, 10), DueDate = new DateTime(2026, 2, 24), ReturnDate = null, IsReturned = false, OverdueCharge = 0, Notes = null },
            new Borrowing { Id = 3, BookId = 4, ReaderId = 1, BorrowDate = new DateTime(2026, 1, 20), DueDate = new DateTime(2026, 2, 3), ReturnDate = new DateTime(2026, 2, 10), IsReturned = true, OverdueCharge = 3.50m, Notes = "Returned 7 days late." },
            new Borrowing { Id = 4, BookId = 2, ReaderId = 3, BorrowDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), ReturnDate = null, IsReturned = false, OverdueCharge = 0, Notes = null },
            new Borrowing { Id = 5, BookId = 5, ReaderId = 2, BorrowDate = new DateTime(2026, 3, 5), DueDate = new DateTime(2026, 3, 19), ReturnDate = null, IsReturned = false, OverdueCharge = 0, Notes = "Reader requested extension." }
        };
        private static int _nextId = 6;

        // GET api/borrowing
        [HttpGet]
        public IActionResult GetAllBorrowings()
        {
            return Ok(_borrowings);
        }

        // GET api/borrowing/{id}
        [HttpGet("{id}")]
        public IActionResult GetBorrowingById(int id)
        {
            var borrowing = _borrowings.FirstOrDefault(b => b.Id == id);
            if (borrowing == null)
                return NotFound(new { message = $"Borrowing record with ID {id} not found." });

            return Ok(borrowing);
        }

        // POST api/borrowing
        [HttpPost]
        public IActionResult CreateBorrowing([FromBody] Borrowing borrowing)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            borrowing.Id = _nextId++;
            _borrowings.Add(borrowing);

            return CreatedAtAction(nameof(GetBorrowingById), new { id = borrowing.Id }, borrowing);
        }

        // PUT api/borrowing/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBorrowing(int id, [FromBody] Borrowing borrowing)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _borrowings.FirstOrDefault(b => b.Id == id);
            if (existing == null)
                return NotFound(new { message = $"Borrowing record with ID {id} not found." });

            existing.BookId = borrowing.BookId;
            existing.ReaderId = borrowing.ReaderId;
            existing.BorrowDate = borrowing.BorrowDate;
            existing.DueDate = borrowing.DueDate;
            existing.ReturnDate = borrowing.ReturnDate;
            existing.IsReturned = borrowing.IsReturned;
            existing.OverdueCharge = borrowing.OverdueCharge;
            existing.Notes = borrowing.Notes;

            return Ok(existing);
        }

        // DELETE api/borrowing/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBorrowing(int id)
        {
            var borrowing = _borrowings.FirstOrDefault(b => b.Id == id);
            if (borrowing == null)
                return NotFound(new { message = $"Borrowing record with ID {id} not found." });

            _borrowings.Remove(borrowing);
            return NoContent();
        }
    }
}
