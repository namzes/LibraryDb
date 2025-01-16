using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryDb.Model.Entities
{
	public class LoanCard
	{
		public int Id { get; set; }
		public required int LoanCardNumber { get; set; }
		public required Customer Customer { get; set; }
		public required List<BookLoanCard> BookLoanCard { get; set; }

		
	}
	public static class LoanCardUtils
	{
		public static async Task<int> GenerateUniqueLoanCardNumber(LibraryContext.LibraryContext context)
		{
			Random random = new Random();
			int loanCardNumber;

			do
			{
				loanCardNumber = random.Next(10000, 100000); // Generate number between 0 and 99999
			}
			while (await context.LoanCards.AnyAsync(lc => lc.LoanCardNumber == loanCardNumber));

			return loanCardNumber;
		}
	}
}
