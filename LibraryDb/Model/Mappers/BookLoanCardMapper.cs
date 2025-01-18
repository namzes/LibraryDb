using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers;

public static class BookLoanCardMapper
{
	public static BookLoanCardGetDto ToBookLoanCardGetDto(this BookLoanCard blc)
	{
		return new BookLoanCardGetDto()
		{
			BookId = blc.Book.Id,
			LoanCardId = blc.LoanCard.Id
		};
	}

}

