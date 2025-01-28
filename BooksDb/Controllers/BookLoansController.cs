using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksDb.Models;
using BooksDb.DTOS;
using BooksDb.Services;

namespace BooksDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookLoansController : ControllerBase
    {
        private readonly BookLoanService _bookLoanService;

        public BookLoansController(BookLoanService bookLoanService)
        {
            _bookLoanService = bookLoanService;
        }

        // GET: api/BookLoans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookLoan>>> GetBookLoans()
        {
            var bookLoans = await _bookLoanService.GetBookLoans();
            return Ok(bookLoans);
        }

        // GET: api/BookLoans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookLoan>> GetBookLoan(int id)
        {
			try
			{
				var bookLoan = await _bookLoanService.GetBookLoan(id);

				return Ok(bookLoan);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { messeage = ex.Message });
			}
		}

        // PUT: api/BookLoans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookLoan(int id, BookLoan bookLoan)
        {
			try
			{
				await _bookLoanService.UpdateBookLoan(id, bookLoan);
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

        [HttpGet("borrower/{borrowerLoanCard}/loans")]
        public async Task<ActionResult<IEnumerable<BookLoan>>> GetLoansByBorrower(int borrowerLoanCard)
        {
			try
			{
				var loans = await _bookLoanService.GetLoansByBorrower(borrowerLoanCard);
				return Ok(loans);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

        [HttpPut("{bookLoanId}/returnloan")]
        public async Task<ActionResult> ReturnLoan(int bookLoanId)
        {
			try
			{
				await _bookLoanService.ReturnLoan(bookLoanId);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}


        // POST: api/BookLoans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookLoan>> PostBookLoan(CreateBookLoanDTO bookLoanDto)
        {
            var bookLoan = await _bookLoanService.AddBookLoan(bookLoanDto);

            return CreatedAtAction("GetBookLoan", new { id = bookLoan.BookLoanId }, bookLoan);
        }

        // DELETE: api/BookLoans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookLoan(int id)
        {
			var success = await _bookLoanService.DeleteBookLoan(id);

			if (!success)
			{
				return NotFound(new { messeage = $"Bookloan with ID {id} not found" });
			}
			return NoContent();
		}
    }
}
