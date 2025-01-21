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
    public class CustomersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public CustomersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerGetDto>>> GetCustomers()
        {
            return await _context.Customers.Select(c => c.ToCustomerGetDto()).ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var loanCard = await _context.LoanCards
	            .Include(lc => lc.Customer)
	            .Where(lc => lc.Customer.Id == id)
	            .FirstOrDefaultAsync();
            
            if (loanCard == null)
            {
                return NotFound();
            }

            var customer = loanCard.Customer;

			return customer.ToCustomerDto(loanCard.ToLoanCardGetDto());
        }
        [HttpGet("{id}/loans")]
        public async Task<ActionResult<CustomerGetLoanDto>> GetCustomerLoans(int id)
        {
			var loanCard = await _context.LoanCards
				.Include(lc => lc.Customer)
				.FirstOrDefaultAsync(lc => lc.Customer.Id == id);

			if (loanCard == null)
	        {
		        return NotFound();
	        }

	        var customer = loanCard.Customer;

	        
	        var loanData = await _context.Loans.Include(l => l.BookLoanCard)
		        .ThenInclude(bc => bc.Book)
		        .ThenInclude(b => b.BookInfo)
		        .Where(l => l.BookLoanCard.LoanCard.Id == loanCard.Id)
		        .Select(l => new BookLoanDate
		        {
			        BookTitle = l.BookLoanCard.Book.BookInfo.Title,
			        LoanDate = l.LoanDate
		        })
		        .ToListAsync();

	        return Ok(customer.ToCustomerGetLoanDto(loanData));
        }

		// PUT: api/Customers/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerPutDto dto)
        {
	        var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

			if (dto.FirstName != null) customer.FirstName = dto.FirstName;
            if (dto.LastName != null) customer.LastName = dto.LastName;
            if (dto.Address != null) customer.Address = dto.Address;
            if (dto.BirthDate.HasValue) customer.BirthDate = dto.BirthDate.Value;


            _context.Entry(customer).State = EntityState.Modified;
	        await _context.SaveChangesAsync();
	        
            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto dto)
        {
	        var customer = dto.ToCustomer();
	        var loanCard = new LoanCard
	        {
		        LoanCardNumber = await LoanCardUtils.GenerateUniqueLoanCardNumber(_context),
		        Customer = customer,
                BookLoanCard = new()
	        };
	        _context.Customers.Add(customer);
            _context.LoanCards.Add(loanCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer.ToCustomerDto(loanCard.ToLoanCardGetDto()));
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
