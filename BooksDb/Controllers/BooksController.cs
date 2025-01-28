using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksDb.Models;
using CreateBooksDb.DTOS;
using BooksDb.DTOS;
using BooksDb.Services;

namespace BooksDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToBookDTO>>> GetBooks()
        {
            var books = await _bookService.GetBooks();

            var booksDtos = books
                .Select(b => b.ToBookDTO())
                .DistinctBy(b => b.ISBN)
                .ToList();

            return Ok(booksDtos);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToBookDTO>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBook(id);
				var bookDto = book.ToBookDTO();

                return Ok(bookDto);
			} 
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
			try
			{
				await _bookService.UpdateBook(id, book);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(CreateBookDTO bookDto)
        {
            var book = await _bookService.AddBook(bookDto);

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
			var success = await _bookService.DeleteBook(id);

			if (!success)
			{
				return NotFound(new { messeage = $"Book with ID {id} not found" });
			}
			return NoContent();
		}
    }
}
