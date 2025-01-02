using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class BookInfo
	{
		public int Id { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? Title { get; set; }
		[Column(TypeName = "varchar(400)")]
		public string? Description { get; set; }
		public decimal Rating { get; set; }
		public List<Book>? Books { get; set; }
		public List<Author>? Authors { get; set; }
	}
}
