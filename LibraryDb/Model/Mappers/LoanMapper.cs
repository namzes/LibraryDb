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
				BookId = loan.BookCustomer?.Book.Id ?? 0,
				BookTitle = loan.BookCustomer?.Book?.BookInfo?.Title ?? "Unknown",
				CustomerId = loan.BookCustomer?.Customer.Id ?? 0,
				CustomerName = $"{loan.BookCustomer?.Customer?.FirstName ?? "Unknown"} {loan.BookCustomer?.Customer?.LastName ?? "Unknown"}",
				LoanDate = loan.LoanDate,
				ExpectedReturnDate = loan.ExpectedReturnDate,
				IsLate = loan.IsLate,
				Returned = loan.Returned
			};
		}
		
	}
}
