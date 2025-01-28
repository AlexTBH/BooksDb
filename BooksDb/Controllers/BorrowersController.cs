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
    public class BorrowersController : ControllerBase
    {
        private readonly BorrowerService _borrowerService;

        public BorrowersController(BorrowerService borrowerService)
        {
            _borrowerService = borrowerService;
        }

        // GET: api/Borrowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrower>>> GetBorrowers()
        {
            var borrowers = await _borrowerService.GetBorrowers();
            return Ok(borrowers);
        }

        // GET: api/Borrowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToBorrowerDTO>> GetBorrower(int id)
        {
			try
			{
				var borrower = await _borrowerService.GetBorrower(id);
				var borrowerDto = borrower.ToBorrowerDTO();

				return Ok(borrowerDto);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { messeage = ex.Message });
			}
		}

        // PUT: api/Borrowers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrower(int id, Borrower borrower)
        {
			try
			{
				await _borrowerService.UpdateBorrower(id, borrower);
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

        // POST: api/Borrowers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Borrower>> PostBorrower(CreateBorrowerDTO borrowerDto)
        {
            var borrower = borrowerDto.ToBorrower();
            var addedBorrower = await _borrowerService.AddBorrower(borrower);

            return CreatedAtAction("GetBorrower", new { id = addedBorrower.BorrowerId }, addedBorrower);
        }

        // DELETE: api/Borrowers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
			var success = await _borrowerService.DeleteBorrower(id);

			if (!success)
			{
				return NotFound(new { messeage = $"Author with ID {id} not found" });
			}
			return NoContent();
		}
    }
}
