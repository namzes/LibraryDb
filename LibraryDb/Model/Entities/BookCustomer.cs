using Microsoft.Build.Framework;

namespace LibraryDb.Model.Entities
{
	public class BookCustomer
	{
		public int Id { get; set; }
		[Required]
		public int BookId { get;set; }
		public Book? Book { get; set; }
		[Required]
		public int CustomerId { get; set; }
		public Customer? Customer { get; set; }
	}
}
