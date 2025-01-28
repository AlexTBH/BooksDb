using BooksDb.DTOS;
using BooksDb.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksDb.Services
{
	public class CopyService
	{
		private readonly AppDbContext _context;

		public CopyService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Copy>> GetCopies()
		{
			return await _context.Copies.ToListAsync();
		}

		public async Task<Copy> GetCopy(int id)
		{
			var copy = await _context.Copies.FindAsync(id);
			if (copy == null)
			{
				throw new KeyNotFoundException($"No copy with the id {id} found");
			}
			return copy;
		}

		public async Task<Copy> UpdateCopy(int id, Copy copy)
		{
			if (id != copy.CopyId)
			{
				throw new ArgumentException("Copy ID not found");
			}

			_context.Entry(copy).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CopyExists(id))
				{
					throw new KeyNotFoundException($"Copy with ID {id} not found");
				}
				else
				{
					throw;
				}
			}

			return copy;
		}

		public async Task<Copy> AddCopy(CreateCopyDTO copyDto)
		{
			var book = await _context.Books
				.FirstOrDefaultAsync(b => b.BookId == copyDto.BookId);

			if (book == null)
			{
				throw new ArgumentException($"No book found with ID {copyDto.BookId}");
			}

			var copy = copyDto.ToCopy();

			_context.Copies.Add(copy);
			await _context.SaveChangesAsync();

			return copy;
		}

		public async Task<bool> DeleteCopy(int id)
		{
			var copy = await _context.Copies.FindAsync(id);

			if (copy == null)
			{
				return false;
			}

			_context.Copies.Remove(copy);
			await _context.SaveChangesAsync();
			return true;
		}

		private bool CopyExists(int id)
		{
			return _context.Copies.Any(e => e.CopyId == id);
		}
	}
}
