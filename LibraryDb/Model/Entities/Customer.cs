using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace LibraryDb.Model.Entities
{
	public class Customer
	{
		public int Id { get; set; }

		[Required, Column(TypeName = "varchar(200)")]
		public required string FirstName { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public required string LastName { get; set; } = string.Empty;

		[Required, Column(TypeName = "varchar(200)")]
		public required string Address { get; set; } = string.Empty;
		[Required]
		public required DateOnly BirthDate { get; set; }
		public List<BookCustomer>? BookCustomers {get; set;}
		
	}
}
