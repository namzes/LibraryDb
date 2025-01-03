using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class Author
	{
		public int Id { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? FirstName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public int BookInfoId { get; set; }
		public BookInfo? BookInfo { get; set; }
	}
}
