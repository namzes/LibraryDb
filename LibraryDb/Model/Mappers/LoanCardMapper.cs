using LibraryDb.Model.DTOs;
using LibraryDb.Model.Entities;

namespace LibraryDb.Model.Mappers
{
	public static class LoanCardMapper
	{
		public static LoanCardGetDto ToLoanCardGetDto(this LoanCard loanCard)
		{
			return new LoanCardGetDto()
			{
				Id = loanCard.Id,
				LoanCardNumber = loanCard.LoanCardNumber
			};
		}

		public static LoanCardWithCustomerDto ToLoanCardWithCustomerDto(this LoanCard loanCard)
		{
			return new LoanCardWithCustomerDto()
			{
				Id = loanCard.Id,
				LoanCardNumber = loanCard.LoanCardNumber,
				Customer = loanCard.Customer.ToCustomerGetDto()
			};
		}
	}
}


