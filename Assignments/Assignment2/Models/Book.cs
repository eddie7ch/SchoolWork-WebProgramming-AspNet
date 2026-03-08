using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Author name must be between 1 and 100 characters.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required.")]
        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "ISBN must be 10 or 13 digits.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50, ErrorMessage = "Genre must not exceed 50 characters.")]
        public string Genre { get; set; } = string.Empty;

        [Range(1, 10000, ErrorMessage = "Total copies must be between 1 and 10000.")]
        public int TotalCopies { get; set; }

        [Range(0, 10000, ErrorMessage = "Available copies must be between 0 and 10000.")]
        public int AvailableCopies { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        [StringLength(100, ErrorMessage = "Publisher name must not exceed 100 characters.")]
        public string? Publisher { get; set; }
    }
}
