using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class BookInfoMapper
	{
		public static BookInfo ToBook(this BookInfoPostDto dto, List<BookInfoAuthor> bookInfoAuthors)
		{
			return new BookInfo()
			{
				Title = dto.Title,
				Description = dto.Description,
				Rating = dto.Rating,
				BookInfoAuthors = bookInfoAuthors
			};
		}
		public static BookInfo ToBook(this BookInfoPostDto dto)
		{
			return dto.ToBook(new List<BookInfoAuthor>());
		}

		public static BookInfoGetDto ToBookInfoGetDto(this BookInfo bookInfo, List<AuthorGetDto> authors)
		{
			return new BookInfoGetDto
			{
				Title = bookInfo.Title,
				Description = bookInfo.Description,
				Rating = bookInfo.Rating,
				BooksInInventory = bookInfo.Books?.Count ?? 0,
				Authors = authors
			};
		}
	}
}
