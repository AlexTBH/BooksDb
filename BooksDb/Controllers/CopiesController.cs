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
    public class CopiesController : ControllerBase
    {
        private readonly CopyService _copyService;

        public CopiesController(CopyService copyService)
        {
            _copyService = copyService;
        }

        // GET: api/Copies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Copy>>> GetCopies()
        {
            var copies = await _copyService.GetCopies();
            return Ok(copies);
        }

        // GET: api/Copies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Copy>> GetCopy(int id)
        {
			try
			{
				var copy = await _copyService.GetCopy(id);
				
				return Ok(copy);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { messeage = ex.Message });
			}

		}

        // PUT: api/Copies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCopy(int id, Copy copy)
        {
			try
			{
				await _copyService.UpdateCopy(id, copy);
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

        // POST: api/Copies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Copy>> PostCopy(CreateCopyDTO copyDto)
        {
            var copy = await _copyService.AddCopy(copyDto);

            return CreatedAtAction("GetCopy", new { id = copy.CopyId }, copy);
        }

        // DELETE: api/Copies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
			var success = await _copyService.DeleteCopy(id);

			if (!success)
			{
				return NotFound(new { messeage = $"Copy with ID {id} not found" });
			}
			return NoContent();
		}
    }
}
