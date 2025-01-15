using System.ComponentModel.DataAnnotations.Schema;
using LibraryDb.Model.Entities;
using Microsoft.Build.Framework;

namespace LibraryDb.Model.DTOs
{

	public class AuthorGetDto
	{
		public int Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		
	}
	public class AuthorGetWithBooksDto
	{
		public int Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public List<string>? WrittenBooks { get; set; }

	}

	public class AuthorPostWithBookInfoDto
	{
		[Required, Column(TypeName = "varchar(200)")]
		public required string FirstName { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public required string LastName { get; set; }
	}
	public class AuthorPostDto
	{
		[Required, Column(TypeName = "varchar(200)")]
		public required string FirstName { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public required string LastName { get; set; }
		public List<int>? BookInfoIdLinks { get; set; }
	}
	public class AuthorPutDto
	{
		[Column(TypeName = "varchar(200)")]
		public string? FirstName { get; set; }
		[Column(TypeName = "varchar(200)")]
		public string? LastName { get; set; }
	}
}
