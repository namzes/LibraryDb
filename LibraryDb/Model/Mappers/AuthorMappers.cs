using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class AuthorMappers
	{
		public static Author ToAuthor(this AuthorPostDto dto)
		{
			return new Author()
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};
		}
	}
}
