using LibraryDb.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryDb.Model.DTOs
{
	public class BookGetDto
	{
		public int Id { get; set; }
		public required string BookTitle { get; set; }
		public required string Isbn { get; set; }
		public int Edition { get; set; }
		public int ReleaseYear { get; set; }
		public bool IsAvailable { get; set; } = true;
		
	}
	public class BookPutDto
	{
		[Column(TypeName = "varchar(20)")]
		public string? Isbn { get; set; }
		public int? Edition { get; set; }
		[Range(1000, 9999, ErrorMessage = "ReleaseYear must be a 4-digit year.")]
		public int? ReleaseYear { get; set; }
		public bool? IsAvailable { get; set; }
		public int? BookInfoId { get; set; }
	}

	public class BookPostDto
	{
		[Required, Column(TypeName = "varchar(20)")]
		public required string Isbn { get; set; }
		[Required]
		public required int Edition { get; set; }
		[Required, Range(1000, 9999, ErrorMessage = "ReleaseYear must be a 4-digit year.")]
		public required int ReleaseYear { get; set; }
		[Required]
		public required int BookInfoId { get; set; }
	}
}
