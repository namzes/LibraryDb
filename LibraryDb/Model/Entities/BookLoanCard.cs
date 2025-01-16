using Microsoft.Build.Framework;

namespace LibraryDb.Model.Entities
{
	public class BookLoanCard
	{
		public int Id { get; set; }
		[Required]
		public required Book Book { get; set; }
		[Required]
		public required LoanCard LoanCard { get; set; }
	}
}
