﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryDb.Model.Entities
{
	public class BookInfo
	{
		public int Id { get; set; }
		[Required, Column(TypeName = "varchar(200)")]
		public required string Title { get; set; }
		[Required, Column(TypeName = "varchar(400)")]
		public required string Description { get; set; }
		[Range(1, 10, ErrorMessage = "Rating must be 1-10")]
		public decimal Rating { get; set; }
		public List<Book>? Books { get; set; }
		public required List<BookInfoAuthor> BookInfoAuthors { get; set; }
	}
}
