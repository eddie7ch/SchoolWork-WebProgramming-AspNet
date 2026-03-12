using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private static List<Book> _books = new List<Book>
        {
            new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565", Genre = "Fiction", TotalCopies = 5, AvailableCopies = 3, PublishedDate = new DateTime(1925, 4, 10), Publisher = "Scribner" },
            new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061935466", Genre = "Fiction", TotalCopies = 4, AvailableCopies = 4, PublishedDate = new DateTime(1960, 7, 11), Publisher = "HarperCollins" },
            new Book { Id = 3, Title = "1984", Author = "George Orwell", ISBN = "9780451524935", Genre = "Dystopian", TotalCopies = 6, AvailableCopies = 2, PublishedDate = new DateTime(1949, 6, 8), Publisher = "Signet Classic" },
            new Book { Id = 4, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "9780132350884", Genre = "Technology", TotalCopies = 3, AvailableCopies = 1, PublishedDate = new DateTime(2008, 8, 1), Publisher = "Prentice Hall" },
            new Book { Id = 5, Title = "The Pragmatic Programmer", Author = "David Thomas", ISBN = "9780135957059", Genre = "Technology", TotalCopies = 2, AvailableCopies = 2, PublishedDate = new DateTime(2019, 9, 23), Publisher = "Addison-Wesley" }
        };
        private static int _nextId = 6;

        // GET api/book
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(_books);
        }

        // GET api/book/{id}
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound(new { message = $"Book with ID {id} not found." });

            return Ok(book);
        }

        // POST api/book
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            book.Id = _nextId++;
            _books.Add(book);

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        // PUT api/book/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _books.FirstOrDefault(b => b.Id == id);
            if (existing == null)
                return NotFound(new { message = $"Book with ID {id} not found." });

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.ISBN = book.ISBN;
            existing.Genre = book.Genre;
            existing.TotalCopies = book.TotalCopies;
            existing.AvailableCopies = book.AvailableCopies;
            existing.PublishedDate = book.PublishedDate;
            existing.Publisher = book.Publisher;

            return Ok(existing);
        }

        // DELETE api/book/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound(new { message = $"Book with ID {id} not found." });

            _books.Remove(book);
            return NoContent();
        }
    }
}
