using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class Book
	{
		public int Id { get; set; }
		[Required, Column(TypeName = "varchar(20)")]
		public required string Isbn { get; set; }
		[Required]
		public int Edition { get; set; }
		[Required, Range(1000, 9999, ErrorMessage = "ReleaseYear must be a 4-digit year.")]
		public int ReleaseYear { get; set; }

		public bool IsAvailable { get; set; } = true;
		[Required]
		public required BookInfo BookInfo { get; set; }
		public required List<BookLoanCard> BookLoanCards { get; set; }
	}
}
