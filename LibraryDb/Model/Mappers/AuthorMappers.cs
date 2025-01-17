using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
		public static List<Author> ToAuthors(this List<AuthorPostWithBookInfoDto> dtoAuthorPostWithBookInfo)
		{
			return dtoAuthorPostWithBookInfo.Select(a => a.ToAuthorWithBookInfo(new())).ToList();
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

		public static Author? GetAuthorByName(this IQueryable<Author> authors, string firstName, string lastName)
		{
			return authors.FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);
		}

		

	}
}
