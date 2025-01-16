using LibraryDb.Model.Entities;

namespace LibraryDb.Model.DTOs
{
	public class LoanCardWithCustomerDto
	{
		public int Id { get; set; }
		public required int LoanCardNumber { get; set; }
		public required CustomerGetDto Customer { get; set; }
	}

	public class LoanCardGetDto
	{
		public int Id { get; set; }
		public required int LoanCardNumber { get; set; }
	}
}
