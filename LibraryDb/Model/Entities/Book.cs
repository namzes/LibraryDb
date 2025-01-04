using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class Book
	{
		public int Id { get; set; }
		[Required, Column(TypeName = "varchar(20)")]
		public string Isbn { get; set; } = string.Empty;
		[Required]
		public int Edition { get; set; }
		[Required, Range(1000, 9999, ErrorMessage = "ReleaseYear must be a 4-digit year.")]
		public int ReleaseYear { get; set; }

		public bool LoanStatus { get; set; } = false;
		[Required]
		public int BookInfoId { get; set; }
		public BookInfo? BookInfo { get; set; }
		public List<BookCustomer>? BookCustomers { get; set; }
	}
}
