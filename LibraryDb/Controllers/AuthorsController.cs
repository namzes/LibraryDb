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
using Microsoft.AspNetCore.Mvc.Formatters;

namespace LibraryDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorGetDto>>> GetAuthors()
        {
            return await _context.Authors.Select(a=> a.ToAuthorGetDto()).ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorGetWithBooksDto>> GetAuthor(int id)
        {
			var authorDto = await _context.Authors
				.Where(a => a.Id == id)
				.Select(a => a.ToAuthorGetDtoWithBooks(a.BookInfoAuthors
					.Select(bia => bia.BookInfo.Title)
					.ToList()))
				.FirstOrDefaultAsync();


			if (authorDto == null)
            {
                return NotFound();
            }

            return Ok(authorDto);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorPutDto dto)
        {
	        var author = await _context.Authors.FindAsync(id);

	        if (author == null)
	        {
                return NotFound();
	        }

            if (dto.FirstName != null) author.FirstName = dto.FirstName;
            if (dto.LastName != null) author.LastName = dto.LastName;

	        _context.Entry(author).State = EntityState.Modified;

	        await _context.SaveChangesAsync();
	        
            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		public async Task<ActionResult<Author>> PostAuthor(AuthorPostDto dto)
		{
			var existingAuthor = await _context.Authors
				.FirstOrDefaultAsync(a => a.FirstName == dto.FirstName && a.LastName == dto.LastName);
			if (existingAuthor != null)
			{
				return Conflict(new { message = "Author already exists." }); 
			}

			var author = dto.ToAuthor(new());

			_context.Authors.Add(author);
			await _context.SaveChangesAsync();

			var bookInfoAuthors = new List<BookInfoAuthor>();

			if (dto.BookInfoIdLinks != null && dto.BookInfoIdLinks.Any())
			{
				var bookInfos = await _context.BookInfos
					.Where(bi => dto.BookInfoIdLinks.Contains(bi.Id))
					.ToListAsync();

				bookInfoAuthors = bookInfos.Select(bookInfo => new BookInfoAuthor
				{
					BookInfo = bookInfo,
					Author = author
				}).ToList();
			}

			_context.BookInfoAuthors.AddRange(bookInfoAuthors);
			await _context.SaveChangesAsync();


			return CreatedAtAction("GetAuthor", new { id = author.Id }, author.ToAuthorGetDto());
		}

		// DELETE: api/Authors/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
