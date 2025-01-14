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
    public class BookInfoAuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BookInfoAuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/BookInfoAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookInfoAuthorGetDto>>> GetBookInfoAuthors()
        {
            return await _context.BookInfoAuthors.Select(bia => bia.ToBookInfoAuthorGetDto()).ToListAsync();
        }

        // GET: api/BookInfoAuthors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookInfoAuthorGetDto>> GetBookInfoAuthor(int id)
        {
            var bookInfoAuthor = await _context.BookInfoAuthors.Select(bia => bia.ToBookInfoAuthorGetDto()).FirstOrDefaultAsync(bia => bia.Id == id);

            if (bookInfoAuthor == null)
            {
                return NotFound();
            }

            return Ok(bookInfoAuthor);
        }


        // POST: api/BookInfoAuthors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookInfoAuthor>> PostBookInfoAuthor(int bookInfoId, int authorId)
        {
	        var bookInfoAuthor = await _context.BookInfoAuthors
		        .Where(bia => bia.BookInfo.Id == bookInfoId && bia.Author.Id == authorId)
		        .FirstOrDefaultAsync();

	        if (bookInfoAuthor != null)
	        {
		        return BadRequest("This relationship already exists.");
	        }

			var bookInfo = await _context.BookInfos.FindAsync(bookInfoId);
	        var author = await _context.Authors.FindAsync(authorId);

	        if (author == null || bookInfo == null)
	        {
		        return NotFound("Author or BookInfo not found.");
	        }

	        bookInfoAuthor = new BookInfoAuthor
	        {
		        BookInfo = bookInfo,
		        Author = author
	        };

	        _context.BookInfoAuthors.Add(bookInfoAuthor);
	        await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookInfoAuthor", new { id = bookInfoAuthor.Id }, bookInfoAuthor.ToBookInfoAuthorGetDto());
        }

        // DELETE: api/BookInfoAuthors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookInfoAuthor(int id)
        {
            var bookInfoAuthor = await _context.BookInfoAuthors.FindAsync(id);
            if (bookInfoAuthor == null)
            {
                return NotFound();
            }

            _context.BookInfoAuthors.Remove(bookInfoAuthor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookInfoAuthorExists(int id)
        {
            return _context.BookInfoAuthors.Any(e => e.Id == id);
        }
    }
}
