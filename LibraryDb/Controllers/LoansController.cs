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
	        var loans = await _context.Loans.Include(l => l.BookCustomer).
		        ThenInclude(bc => bc.Book).
		        ThenInclude(b => b.BookInfo).
		        Include(l => l.BookCustomer!.Customer).
		        ToListAsync();


            var loanDtos = _context.Loans.Select(loan => loan.ToLoanGetDto()).ToList();

            return Ok(loanDtos);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanGetDto>> GetLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);

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

            loan.LoanDate = dto.LoanDate;
            loan.ExpectedReturnDate = dto.ExpectedReturnDate;
            loan.ActualReturnDate = dto.ActualReturnDate;
            loan.Returned = dto.Returned;
            

            _context.Entry(loan).State = EntityState.Modified; 
            await _context.SaveChangesAsync();
	        
            return NoContent();
        }

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanGetDto>> PostLoan(LoanPostDto loanDto)
        {
	        var book = await _context.Books.FindAsync(loanDto.BookId);

	        if (book == null)
	        {
		        return NotFound();
	        }

			if (!book.IsAvailable)
	        {
		        return Conflict(new { Message = "The book is currently not available for loan." });
	        }

			var customer = await _context.Customers.FindAsync(loanDto.CustomerId);

	        if (customer == null)
	        {
		        return NotFound();
	        }
	        book.IsAvailable = false;
	        var bc = new BookCustomer()
	        {
		        Customer = customer,
		        Book = book
	        };

            _context.BooksCustomers.Add(bc); 

            var loan = new Loan()
            {
	            LoanDate = DateOnly.FromDateTime(DateTime.UtcNow),
				ExpectedReturnDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10),
				BookCustomer = bc
			};
            

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetLoan", new { id = loan.Id }, loan.ToLoanGetDto());
        }

        [HttpPatch("return/{id}")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
	        var loan = await _context.Loans
		        .Include(l => l.BookCustomer)
		        .ThenInclude(bc => bc.Book)
		        .FirstOrDefaultAsync(l => l.Id == id);
	        if (loan == null)
	        {
		        return NotFound();
	        }
			if (loan.BookCustomer.Book.IsAvailable)
	        {
		        return Conflict(new { Message = "The book is already available and can not be returned" });
			}
	        

            loan.Returned = true;
            loan.BookCustomer.Book.IsAvailable = true;
            loan.ActualReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
            loan.IsLate = loan.Returned && loan.ActualReturnDate.HasValue &&
                          loan.ActualReturnDate > loan.ExpectedReturnDate;

            _context.Loans.Entry(loan).State = EntityState.Modified;
            _context.Books.Entry(loan.BookCustomer.Book).State = EntityState.Modified;

			await _context.SaveChangesAsync();

            return Ok(loan.ToLoanGetDto());
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

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
