using BooksDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BooksDb.Services
{
	public class AuthorService
	{
		private readonly AppDbContext _context;

		public AuthorService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Author>> GetAuthors()
		{
			return await _context.Authors.ToListAsync();
		}

		public async Task<Author> GetAuthor(int id)
		{
			var author = await _context.Authors.FindAsync(id);

			if (author == null)
			{
				throw new KeyNotFoundException($"Author with ID {id} not found");
			}

			return author;
		}

		public async Task<Author> UpdateAuthor(int id, Author author)
		{
			if (id != author.AuthorId)
			{
				throw new ArgumentException("Author ID not found");
			}

			_context.Entry(author).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AuthorExists(id))
				{
					throw new KeyNotFoundException($"Author with ID {id} not found");
				}
				else
				{
					throw;
				}
			}

			return author;
		}

		private bool AuthorExists(int id)
		{
			return _context.Authors.Any(e => e.AuthorId == id);
		}

		public async Task<Author> AddAuthor(Author author)
		{
			_context.Authors.Add(author);
			await _context.SaveChangesAsync();
			return author;
		}

		public async Task<bool> DeleteAuthor(int id)
		{
			var author = await _context.Authors.FindAsync(id);
			if (author == null)
			{
				return false;
			}

			_context.Authors.Remove(author);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
