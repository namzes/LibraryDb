using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryDb.Model.Mappers
{
	public static class CustomerMapper
	{
		
		public static Customer ToCustomer(this CustomerDto dto)
		{
			return new Customer
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Address = dto.Address,
				BirthDate = dto.BirthDate
			};
		}
		public static CustomerDto ToCustomerDto(this Customer customer)
		{
			return new CustomerDto
			{
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Address = customer.Address,
				BirthDate = customer.BirthDate
			};
		}
		public static CustomerGetLoanDto ToCustomerGetLoanDto(this Customer customer, List<string> bookTitles, List<DateOnly> loanDates)
		{
			return new CustomerGetLoanDto
			{
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Address = customer.Address,
				BirthDate = customer.BirthDate,
				BookTitles = bookTitles,
				LoanDates = loanDates
				
			};
		}

		
	}
}
