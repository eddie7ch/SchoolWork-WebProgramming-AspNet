using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Borrowing
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "BookId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BookId must be a positive integer.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "ReaderId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ReaderId must be a positive integer.")]
        public int ReaderId { get; set; }

        [Required(ErrorMessage = "Borrow date is required.")]
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        [Range(0, double.MaxValue, ErrorMessage = "Overdue charge must be a non-negative value.")]
        public decimal OverdueCharge { get; set; } = 0;

        [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}
