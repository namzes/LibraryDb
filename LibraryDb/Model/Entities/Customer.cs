using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class Customer
	{
		public int Id { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? FirstName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? LastName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? Address { get; set; }
		public List<BookCustomer>? BookCustomers {get; set;}
		
	}
}
