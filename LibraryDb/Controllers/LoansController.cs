using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDb.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDb.Model.Entities;
using LibraryDb.Model.LibraryContext;
using LibraryDb.Model.Mappers;

namespace LibraryDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanGetDto>>> GetLoans()
        {
	        var loans = await _context.Loans
		        .Include(l => l.BookLoanCard)
		        .ThenInclude(blc => blc.Book)
		        .ThenInclude(b => b.BookInfo)
		        .Include(l => l.BookLoanCard.LoanCard)
		        .ThenInclude(lc => lc.Customer)
		        .ToListAsync();


            var loanDtos = _context.Loans.Select(loan => loan.ToLoanGetDto()).ToList();

            return Ok(loanDtos);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanGetDto>> GetLoan(int id)
        {
	        var loan = await _context.Loans.Include(l => l.BookLoanCard)
		        .ThenInclude(blc => blc.Book)
		        .ThenInclude(b => b.BookInfo)
		        .Include(l => l.BookLoanCard.LoanCard)
		        .ThenInclude(lc => lc.Customer)
		        .FirstOrDefaultAsync(l => l.Id == id);


			if (loan == null)
            {
                return NotFound();
            }

            return loan.ToLoanGetDto();
        }

        // PUT: api/Loans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, LoanPutDto dto)
        {
	        var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            if (dto.LoanDate.HasValue) loan.LoanDate = dto.LoanDate.Value;
            if (dto.ExpectedReturnDate.HasValue) loan.ExpectedReturnDate = dto.ExpectedReturnDate.Value;
            if (dto.ActualReturnDate.HasValue) loan.ActualReturnDate = dto.ActualReturnDate.Value;
            if (dto.Returned.HasValue) loan.Returned = dto.Returned.Value;
            

            _context.Entry(loan).State = EntityState.Modified; 
            await _context.SaveChangesAsync();
	        
            return NoContent();
        }

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanGetDto>> PostLoan(LoanPostDto loanDto)
        {
	        var book = await _context.Books.Include(b => b.BookInfo).FirstOrDefaultAsync(b => b.Id ==loanDto.BookId);

	        if (book == null)
	        {
		        return NotFound();
	        }

			if (!book.IsAvailable)
	        {
				return Conflict(new { message = "Book is currently not available for loan." });
			}

			var loanCard = await _context.LoanCards.Include(lc => lc.Customer).FirstOrDefaultAsync(lc => lc.Id ==loanDto.LoanCardId);

	        if (loanCard == null)
	        {
		        return NotFound();
	        }

	        book.IsAvailable = false;
	        var bc = new BookLoanCard()
	        {
		        LoanCard = loanCard,
		        Book = book
	        };

            _context.BooksCustomers.Add(bc); 

            var loan = new Loan()
            {
	            LoanDate = DateOnly.FromDateTime(DateTime.UtcNow),
				ExpectedReturnDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10),
				BookLoanCard = bc
			};
            

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetLoan", new { id = loan.Id }, loan.ToLoanGetDto());
        }

        [HttpPatch("return/{id}")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
	        var loan = await _context.Loans
		        .Include(l => l.BookLoanCard)
		        .ThenInclude(bcl => bcl.Book)
		        .Include(l => l.BookLoanCard)
		        .ThenInclude(bcl => bcl.LoanCard)
		        .FirstOrDefaultAsync(l => l.Id == id);
	        if (loan == null)
	        {
		        return NotFound();
	        }
			if (loan.BookLoanCard.Book.IsAvailable)
	        {
		        return Conflict(new { Message = "The book is already available and can not be returned" });
			}
	        

            loan.Returned = true;
            loan.BookLoanCard.Book.IsAvailable = true;
            loan.ActualReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
            loan.IsLate = loan.Returned && loan.ActualReturnDate.HasValue &&
                          loan.ActualReturnDate > loan.ExpectedReturnDate;

            _context.Loans.Entry(loan).State = EntityState.Modified;
            _context.Books.Entry(loan.BookLoanCard.Book).State = EntityState.Modified;

			await _context.SaveChangesAsync();

            return Ok(loan.ToLoanGetDto());
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.Include(l => l.BookLoanCard)
	            .ThenInclude(blc => blc.Book).FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            loan.BookLoanCard.Book.IsAvailable = true;

			_context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
