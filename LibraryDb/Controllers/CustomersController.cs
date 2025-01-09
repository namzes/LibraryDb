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
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            return await _context.Customers.Select(c => c.ToCustomerDto()).ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer.ToCustomerDto();
        }
        [HttpGet("{id}/loans")]
        public async Task<ActionResult<CustomerGetLoanDto>> GetCustomerLoans(int id)
        {
	        var customer = await _context.Customers.FindAsync(id);
	        if (customer == null)
	        {
		        return NotFound();
	        }

	        var loanData = await _context.Loans.Include(l => l.BookCustomer)
		        .ThenInclude(bc => bc.Book)
		        .ThenInclude(b => b.BookInfo)
		        .Where(l => l.BookCustomer.Customer.Id == customer.Id)
		        .Select(l => new BookLoanDate
		        {
			        BookTitle = l.BookCustomer.Book.BookInfo.Title,
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto dto)
        {
	        var customer = dto.ToCustomer();
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer.ToCustomerDto());
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
