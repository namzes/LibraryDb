using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class Book
	{
		public int Id { get; set; }
		[Column(TypeName = "varchar(20)")]
		public string? Isbn { get; set; }
		public int Edition { get; set; }
		public int ReleaseYear { get; set; }
		public bool LoanStatus { get; set; }
		public int BookInfoId { get; set; }
		public BookInfo? BookInfo { get; set; }
		public List<BookCustomer>? BookCustomers { get; set; }
	}
}
