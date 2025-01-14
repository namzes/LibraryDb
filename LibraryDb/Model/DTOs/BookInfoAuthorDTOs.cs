using LibraryDb.Model.Entities;

namespace LibraryDb.Model.DTOs
{
	public class BookInfoAuthorGetDto
	{
		public int Id { get; set; }
		public int BookInfoId { get; set; }
		public required string BookTitle { get; set; }
		public int AuthorId { get; set; }
		public required string AuthorName { get; set; }
		
	}

	public class BookInfoAuthorPutDto
	{
		public int BookInfoId { get; set; }
		public int AuthorId { get; set; }
	}
}
