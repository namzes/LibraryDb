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
    public class BookInfosController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BookInfosController(LibraryContext context)
        {
            _context = context;
        }

		// GET: api/BookInfos
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookInfoGetDto>>> GetBookInfos()
		{
			var bookInfos = await _context.BookInfos
				.Select(bi => new BookInfoGetDto
				{
					Title = bi.Title,
					Description = bi.Description,
					Rating = bi.Rating,
					BooksInInventory = bi.Books != null ? bi.Books.Count : 0,
					Authors = bi.BookInfoAuthors == null
						? new List<AuthorGetDto>()
						: bi.BookInfoAuthors.Select(bia => new AuthorGetDto
						{
							FirstName = bia.Author.FirstName,
                            LastName = bia.Author.LastName
						}).ToList()
				})
				.ToListAsync();

			return Ok(bookInfos);
		}

        // GET: api/BookInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookInfoGetDto>> GetBookInfo(int id)
        {
			var bookInfo = await _context.BookInfos
				.Where(bi => bi.Id == id)
				.Select(bi => new BookInfoGetDto
				{
					Title = bi.Title,
					Description = bi.Description,
					Rating = bi.Rating,
					BooksInInventory = bi.Books != null ? bi.Books.Count : 0,
					Authors = bi.BookInfoAuthors == null
						? new List<AuthorGetDto>()
						: bi.BookInfoAuthors.Select(bia => new AuthorGetDto
						{
							FirstName = bia.Author.FirstName,
							LastName = bia.Author.LastName
						}).ToList()
				}).FirstOrDefaultAsync();

			if (bookInfo == null)
			{
				return NotFound();
			}

			return Ok(bookInfo);
		}

        // PUT: api/BookInfos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookInfo(int id, BookInfoPutDto bookInfo)
        {
	        var book = await _context.BookInfos.FindAsync(id);
            if (book == null)
            {
	            return NotFound();
            }

            if (bookInfo.Title != null) book.Title = bookInfo.Title;
            if (bookInfo.Description != null) book.Description = bookInfo.Description;
            if (bookInfo.Rating.HasValue) book.Rating = bookInfo.Rating.Value;

            _context.Entry(bookInfo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
                
            return NoContent();
        }

        // POST: api/BookInfos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookInfo>> PostBookInfo(BookInfoPostDto dto)
        {
	        var bookInfo = dto.ToBook();
	        
	        var authors = dto.Authors?.Select(a => a.ToAuthor()).ToList() ?? new List<Author>();


	        for (int i = 0; i < authors.Count; i++)
			{
				var existingAuthor = await _context.Authors
					.FirstOrDefaultAsync(a => a.FirstName == authors[i].FirstName && a.LastName == authors[i].LastName);

				if (existingAuthor != null)
				{
                    authors[i] = existingAuthor;
				}
			}


			List<BookInfoAuthor> bookInfoAuthors = authors.Select(author => new BookInfoAuthor
	        {
		        BookInfo = bookInfo,
		        Author = author
	        }).ToList();

	        var newAuthors = authors.Where(a => a.Id == 0).ToList();
			_context.Authors.AddRange(newAuthors);

			_context.BookInfos.Add(bookInfo); 
			_context.BookInfoAuthors.AddRange(bookInfoAuthors);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookInfo", new { id = bookInfo.Id }, bookInfo);
        }

        // DELETE: api/BookInfos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookInfo(int id)
        {
            var bookInfo = await _context.BookInfos.FindAsync(id);
            if (bookInfo == null)
            {
                return NotFound();
            }

            _context.BookInfos.Remove(bookInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
		private bool BookInfoExists(int id)
        {
            return _context.BookInfos.Any(e => e.Id == id);
        }
    }
}
