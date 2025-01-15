using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class BookInfoAuthorMapper
	{
		public static BookInfoAuthorGetDto ToBookInfoAuthorGetDto(this BookInfoAuthor bookInfoAuthor)
		{
			return new BookInfoAuthorGetDto
			{
				Id = bookInfoAuthor.Id,
				BookInfoId = bookInfoAuthor.BookInfo.Id,
				BookTitle = bookInfoAuthor.BookInfo.Title,
				AuthorId = bookInfoAuthor.Author.Id,
				AuthorName = bookInfoAuthor.Author.FirstName + " " + bookInfoAuthor.Author.LastName
			};
		}
	}
}
