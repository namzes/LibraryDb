using LibraryDb.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryDb.Model.DTOs
{
	public class BookGetDto
	{
		public required int Id { get; set; }
		public required string BookTitle { get; set; }
		public required string Isbn { get; set; }
		public int Edition { get; set; }
		public int ReleaseYear { get; set; }
		public bool IsAvailable { get; set; } = true;
		
	}
}
