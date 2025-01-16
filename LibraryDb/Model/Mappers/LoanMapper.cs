using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class LoanMapper
	{
		public static LoanGetDto ToLoanGetDto(this Loan loan)
		{
			

			return new LoanGetDto()
			{
				LoanId = loan.Id,
				BookId = loan.BookLoanCard.Book.Id,
				BookTitle = loan.BookLoanCard.Book.BookInfo.Title,
				LoanCardNumber = loan.BookLoanCard.LoanCard.LoanCardNumber,
				CustomerId = loan.BookLoanCard.LoanCard.Customer.Id,
				CustomerName = $"{loan.BookLoanCard.LoanCard.Customer.FirstName} {loan.BookLoanCard.LoanCard.Customer.LastName}",
				LoanDate = loan.LoanDate,
				ExpectedReturnDate = loan.ExpectedReturnDate,
				IsLate = loan.IsLate,
				Returned = loan.Returned
			};
		}
		
	}
}
