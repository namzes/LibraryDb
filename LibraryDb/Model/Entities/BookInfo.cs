using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class BookInfo
	{
		public int Id { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public string Title { get; set; } = string.Empty;
		[Required, Column(TypeName = "varchar(400)")]
		public string Description { get; set; } = string.Empty;
		[Range(1, 10, ErrorMessage = "Rating must be 1-10")]
		public decimal Rating { get; set; }
		public List<Book>? Books { get; set; }
		public List<Author>? Authors { get; set; }
	}
}
