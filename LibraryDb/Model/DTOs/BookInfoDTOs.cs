using LibraryDb.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryDb.Model.DTOs
{
	public class BookInfoGetDto
	{
		public required string Title { get; set; }
		public required string Description { get; set; }
		public decimal Rating { get; set; }
		public int? BooksInInventory { get; set; }
		public List<AuthorGetDto>? Authors { get; set; }
	}

	public class BookInfoPostDto
	{
		[Required, Column(TypeName = "varchar(200)")]
		public required string Title { get; set; }
		[Required, Column(TypeName = "varchar(400)")]
		public required string Description { get; set; }
		[Range(1, 10, ErrorMessage = "Rating must be 1-10")]
		public decimal Rating { get; set; }
		public List<AuthorPostDto>? Authors { get; set; }
	}
	public class BookInfoPutDto
	{
		[Column(TypeName = "varchar(200)")]
		public string? Title { get; set; }
		[Column(TypeName = "varchar(400)")]
		public string? Description { get; set; }
		[Range(1, 10, ErrorMessage = "Rating must be 1-10")]
		public decimal? Rating { get; set; }
	}
}
