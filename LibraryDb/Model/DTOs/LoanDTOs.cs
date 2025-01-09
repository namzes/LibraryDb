using Microsoft.Build.Framework;

namespace LibraryDb.Model.DTOs
{
	public class LoanGetDto
	{
		public int LoanId { get; set; }
		public int BookId { get; set; }
		public string? BookTitle { get; set; }
		public int CustomerId { get; set; }
		public string? CustomerName { get; set; }
		public DateOnly LoanDate { get; set; }
		public DateOnly ExpectedReturnDate { get; set; }
		public DateOnly? ActualReturnDate { get; set; }
		public bool IsLate { get; set; }
		public bool Returned { get; set; }
	}
	

	public class LoanPostDto
	{
		[Required]
		public int BookId { get; set; }
		[Required]
		public int CustomerId { get; set; }
	}

	public class LoanPutDto
	{
		public DateOnly LoanDate { get; set; }
		public DateOnly ExpectedReturnDate { get; set; }
		public DateOnly? ActualReturnDate { get; set; }
		public bool Returned { get; set; }

	}
}
