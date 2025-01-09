using LibraryDb.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.DTOs
{
	public class CustomerDto
	{
		[Column(TypeName = "varchar(200)")]
		public required string FirstName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public required string LastName { get; set; }

		[Column(TypeName = "varchar(200)")]
		public required string Address { get; set; }
		public required DateOnly BirthDate { get; set; }
	}

	public class CustomerPutDto
	{
		[Column(TypeName = "varchar(200)")]
		public string? FirstName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? LastName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? Address { get; set; }
		public DateOnly? BirthDate { get; set; }
	}
	public class CustomerGetLoanDto
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string Address { get; set; }
		public required DateOnly BirthDate { get; set; }
		public List<string>? BookTitles { get; set; }
		public List<DateOnly>? LoanDates { get; set; }
		
	}
}
