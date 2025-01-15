using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class AuthorMappers
	{
		public static Author ToAuthor(this AuthorPostDto dto, List<BookInfoAuthor> bookInfoAuthors)
		{
			return new Author()
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				BookInfoAuthors = bookInfoAuthors
			};
		}
		public static Author ToAuthorWithBookInfo(this AuthorPostWithBookInfoDto dto, List<BookInfoAuthor> bookInfoAuthors)
		{
			return new Author()
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				BookInfoAuthors = bookInfoAuthors
			};
		}
		public static AuthorGetDto ToAuthorGetDto(this Author author)
		{
			return new AuthorGetDto
			{
				Id = author.Id,
				FirstName = author.FirstName,
				LastName = author.LastName,
			};
		}
		public static AuthorGetWithBooksDto ToAuthorGetDtoWithBooks(this Author author, List<string>? writtenBooks)
		{
			return new AuthorGetWithBooksDto
			{
				Id = author.Id,
				FirstName = author.FirstName,
				LastName = author.LastName,
				WrittenBooks = writtenBooks ?? new()
			};
		}
	}
}
