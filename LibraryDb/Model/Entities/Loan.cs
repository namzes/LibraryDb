using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace LibraryDb.Model.Entities
{
	public class Loan
	{
		public int Id { get; set; }
		[Required]
		public required DateOnly LoanDate { get; set; }
		[Required]
		public DateOnly ExpectedReturnDate { get; set; }
		public DateOnly? ActualReturnDate { get; set; }
		public bool Returned { get; set; } = false;
		public bool IsLate;
		[Required]
		public required BookCustomer BookCustomer { get; set; }

	}


}
