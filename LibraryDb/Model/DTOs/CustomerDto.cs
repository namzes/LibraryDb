using LibraryDb.Model.Entities;

namespace LibraryDb.Model.DTOs
{
	public class CustomerCreateDto
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string Address { get; set; }
		public required DateOnly BirthDate { get; set; }
	}

	public class CustomerGetLoanDto
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string Address { get; set; }
		public required DateOnly BirthDate { get; set; }
		public List<string>? BookTitle { get; set; }
		public List<DateOnly>? LoanDate { get; set; }
		
	}
}
