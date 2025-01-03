using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace LibraryDb.Model.Entities
{
	public class Loan
	{
		public int Id { get; set; }
		[Required]
		public DateTime LoanDate { get; set; } = DateTime.UtcNow;
		[Required]
		public DateTime ExpectedReturnDate { get; set; }
		public DateTime? ActualReturnDate { get; set; }
		public bool Returned { get; set; } = false;
		public bool LateReturn => Returned && ActualReturnDate.HasValue && ActualReturnDate > ExpectedReturnDate;
		public int BookCustomerId { get; set; }
		public BookCustomer? BookCustomer { get; set; }
		
	}
}
