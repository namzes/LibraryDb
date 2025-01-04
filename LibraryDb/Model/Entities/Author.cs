using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace LibraryDb.Model.Entities
{
	public class Author
	{
		public int Id { get; set; }

		[Required, Column(TypeName = "varchar(200)")]
		public string FirstName { get; set; } = string.Empty;
		[Required, Column(TypeName = "varchar(200)")]
		public string LastName { get; set; } = string.Empty;
		public int BookInfoId { get; set; }
		public BookInfo? BookInfo { get; set; }
	}
}
