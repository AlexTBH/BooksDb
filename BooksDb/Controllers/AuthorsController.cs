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
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authors = await _authorService.GetAuthors();
            return Ok(authors);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToAuthorDTO>> GetAuthor(int id)
        {
            try
            {
				var author = await _authorService.GetAuthor(id);
                var authorDto = author.ToAuthorDTO;

                return Ok(authorDto);
			}
            catch (KeyNotFoundException ex)
            {
                return NotFound(new {messeage = ex.Message});
            }
		}

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
			try
			{
				await _authorService.UpdateAuthor(id, author);
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(CreateAuthorDTO authorDto)
        {
            var author = authorDto.ToAuthor();
            var addedAuthor = await _authorService.AddAuthor(author);
            return CreatedAtAction("GetAuthor", new { id = addedAuthor.AuthorId }, addedAuthor);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var success = await _authorService.DeleteAuthor(id);

            if (!success)
            {
                return NotFound(new { messeage = $"Author with ID {id} not found" });
            }
            return NoContent();
        }

    }
}
