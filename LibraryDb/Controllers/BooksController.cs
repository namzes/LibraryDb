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
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookGetDto>>> GetBooks()
        {
	        return await _context.Books.Include(b => b.BookInfo).Select(b => b.ToBookGetDto(b.BookInfo.Title)).ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookGetDto>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.BookInfo).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book.ToBookGetDto(book.BookInfo.Title);
        }
        [HttpGet("search/{searchTitle}")]
        public async Task<ActionResult<IEnumerable<BookGetDto>>> GetBookBySearchTitle(string searchTitle)
        {
			if (string.IsNullOrEmpty(searchTitle))
			{
				return NotFound();
			}

			var books = await _context.Books
		        .Include(b => b.BookInfo)
		        .Where(b => b.BookInfo.Title.ToLower().Contains(searchTitle.ToLower()))
		        .Select(b => b.ToBookGetDto(b.BookInfo.Title))
		        .ToListAsync();


	        return Ok(books);
        }

		// PUT: api/Books/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookPutDto dto)
        {
            
	        var book = await _context.Books.Include(b => b.BookInfo).FirstOrDefaultAsync(b=> id == b.Id);

            if (book == null)
            {
                return NotFound();
            }

            if (dto.Isbn != null) book.Isbn = dto.Isbn;
            if (dto.Edition.HasValue) book.Edition = dto.Edition.Value;
            if (dto.ReleaseYear.HasValue) book.ReleaseYear = dto.ReleaseYear.Value;
            if (dto.IsAvailable.HasValue) book.IsAvailable = dto.IsAvailable.Value;

            if (dto.BookInfoId != null)
            {
				var bookInfo = await _context.BookInfos.FindAsync(dto.BookInfoId);

	            if (bookInfo != null) book.BookInfo = bookInfo;
            }
				 
           
			_context.Entry(book).State = EntityState.Modified;
	        await _context.SaveChangesAsync();
	        
            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookGetDto>> PostBook(BookPostDto dto)
        {
	        var bookInfo = await _context.BookInfos.FindAsync(dto.BookInfoId);
	        if (bookInfo == null)
	        {
		        return NotFound("Book Information with that Id doesn't exist");
	        }

	        var book = dto.ToBook(bookInfo);
            
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book.ToBookGetDto(bookInfo.Title));
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
