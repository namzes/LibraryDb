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
    public class LoanCardsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoanCardsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/LoanCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanCardGetDto>>> GetLoanCards()
        {
            return await _context.LoanCards.Select(lc => lc.ToLoanCardGetDto()).ToListAsync();
        }

        // GET: api/LoanCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanCardWithCustomerDto>> GetLoanCard(int id)
        {
			var loanCard = await _context.LoanCards
				.Include(lc => lc.Customer)
				.FirstOrDefaultAsync(lc => lc.Id == id);

			if (loanCard == null)
            {
                return NotFound();
            }


            return loanCard.ToLoanCardWithCustomerDto();
        }

        private bool LoanCardExists(int id)
        {
            return _context.LoanCards.Any(e => e.Id == id);
        }
    }
}
