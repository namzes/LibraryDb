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
    public class BookLoanCardsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BookLoanCardsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/BookLoanCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookLoanCardGetDto>>> GetBookLoanCards()
        {
            return await _context.BookLoanCards
	            .Include(blc => blc.Book)
	            .Include(blc => blc.LoanCard)
	            .Select(blc => blc.ToBookLoanCardGetDto())
	            .ToListAsync();
        }

        
    }
}
