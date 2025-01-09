using LibraryDb.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryDb.Model.DTOs
{
	public class BookDto
	{
		[Required, Column(TypeName = "varchar(20)")]
		public required string Isbn { get; set; }
		[Required]
		public int Edition { get; set; }
		[Required, Range(1000, 9999, ErrorMessage = "ReleaseYear must be a 4-digit year.")]
		public int ReleaseYear { get; set; }

		public bool IsAvailable { get; set; } = true;
		[Required]
		public required BookInfo BookInfo { get; set; }
	}
}
