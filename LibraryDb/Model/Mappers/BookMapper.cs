using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class BookMapper
	{
		public static Book ToBook(this BookPostDto dto, BookInfo bookInfo)
		{
			return new Book
			{
				Isbn = dto.Isbn,
				Edition = dto.Edition,
				ReleaseYear = dto.ReleaseYear,
				BookInfo = bookInfo
			};
		}
		public static BookGetDto ToBookGetDto(this Book book, string bookTitle)
		{
			return new BookGetDto
			{
				BookTitle = bookTitle,
				Id = book.Id,
				Isbn = book.Isbn,
				Edition = book.Edition,
				ReleaseYear = book.ReleaseYear,
				IsAvailable = book.IsAvailable
			};
		}
	}
}
