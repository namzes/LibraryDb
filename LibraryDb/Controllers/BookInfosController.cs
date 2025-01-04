using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDb.Model.Entities;
using LibraryDb.Model.LibraryContext;

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
        public async Task<ActionResult<IEnumerable<BookInfo>>> GetBookInfos()
        {
            return await _context.BookInfos.ToListAsync();
        }

        // GET: api/BookInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookInfo>> GetBookInfo(int id)
        {
            var bookInfo = await _context.BookInfos.FindAsync(id);

            if (bookInfo == null)
            {
                return NotFound();
            }

            return bookInfo;
        }

        // PUT: api/BookInfos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookInfo(int id, BookInfo bookInfo)
        {
            if (id != bookInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookInfoExists(id))
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

        // POST: api/BookInfos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookInfo>> PostBookInfo(BookInfo bookInfo)
        {
            _context.BookInfos.Add(bookInfo);
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
