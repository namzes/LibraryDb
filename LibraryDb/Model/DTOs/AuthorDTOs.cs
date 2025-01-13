using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace LibraryDb.Model.DTOs
{
	public class AuthorGetDto
	{
		public int Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		
	}
	public class AuthorPostDto
	{
		[Required, Column(TypeName = "varchar(200)")]
		public required string FirstName { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public required string LastName { get; set; }
	}
}
