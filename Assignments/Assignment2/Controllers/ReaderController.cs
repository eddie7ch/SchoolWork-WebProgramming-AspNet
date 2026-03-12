using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private static List<Reader> _readers = new List<Reader>
        {
            new Reader { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@email.com", PhoneNumber = "403-555-1001", MembershipNumber = "MEM0001", MembershipStartDate = new DateTime(2024, 1, 15), MembershipExpiryDate = new DateTime(2026, 1, 15), Address = "123 Main St, Calgary, AB", IsActive = true },
            new Reader { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob.smith@email.com", PhoneNumber = "403-555-1002", MembershipNumber = "MEM0002", MembershipStartDate = new DateTime(2024, 3, 20), MembershipExpiryDate = new DateTime(2026, 3, 20), Address = "456 Oak Ave, Calgary, AB", IsActive = true },
            new Reader { Id = 3, FirstName = "Carol", LastName = "White", Email = "carol.white@email.com", PhoneNumber = "403-555-1003", MembershipNumber = "MEM0003", MembershipStartDate = new DateTime(2023, 6, 1), MembershipExpiryDate = new DateTime(2025, 6, 1), Address = "789 Pine Rd, Calgary, AB", IsActive = false }
        };
        private static int _nextId = 4;

        // GET api/reader
        [HttpGet]
        public IActionResult GetAllReaders()
        {
            return Ok(_readers);
        }

        // GET api/reader/{id}
        [HttpGet("{id}")]
        public IActionResult GetReaderById(int id)
        {
            var reader = _readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
                return NotFound(new { message = $"Reader with ID {id} not found." });

            return Ok(reader);
        }

        // POST api/reader
        [HttpPost]
        public IActionResult CreateReader([FromBody] Reader reader)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            reader.Id = _nextId++;
            _readers.Add(reader);

            return CreatedAtAction(nameof(GetReaderById), new { id = reader.Id }, reader);
        }

        // PUT api/reader/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateReader(int id, [FromBody] Reader reader)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _readers.FirstOrDefault(r => r.Id == id);
            if (existing == null)
                return NotFound(new { message = $"Reader with ID {id} not found." });

            existing.FirstName = reader.FirstName;
            existing.LastName = reader.LastName;
            existing.Email = reader.Email;
            existing.PhoneNumber = reader.PhoneNumber;
            existing.MembershipNumber = reader.MembershipNumber;
            existing.MembershipStartDate = reader.MembershipStartDate;
            existing.MembershipExpiryDate = reader.MembershipExpiryDate;
            existing.Address = reader.Address;
            existing.IsActive = reader.IsActive;

            return Ok(existing);
        }

        // DELETE api/reader/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteReader(int id)
        {
            var reader = _readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
                return NotFound(new { message = $"Reader with ID {id} not found." });

            _readers.Remove(reader);
            return NoContent();
        }
    }
}
