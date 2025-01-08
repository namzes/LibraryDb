using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class CustomerMapper
	{
		
		public static Customer ToCustomer(this CustomerCreateDto dto)
		{
			return new Customer
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Address = dto.Address,
				BirthDate = dto.BirthDate

			};
		}

		public static CustomerGetLoanDto ToCustomerGetLoanDto(this Customer customer)
		{
			return new CustomerGetLoanDto
			{
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Address = customer.Address,
				BirthDate = customer.BirthDate
			};
		}
	}
}
