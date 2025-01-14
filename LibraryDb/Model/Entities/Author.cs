using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace LibraryDb.Model.Entities
{
	public class Author
	{
		public int Id { get; set; }

		[Required, Column(TypeName = "varchar(200)")]
		public required string FirstName { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public required string LastName { get; set; }
		public required List<BookInfoAuthor> BookInfoAuthors { get; set; }
	}
}
