using BooksDb.DTOS;
using BooksDb.Models;
using CreateBooksDb.DTOS;
using Microsoft.EntityFrameworkCore;

namespace BooksDb.Services
{
	public class BookService
	{
		private readonly AppDbContext _context;

		public BookService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Book>> GetBooks()
		{
			var books = await _context.Books
				.Include(b => b.Authors)
				.ToListAsync();

			return books;
		}

		public async Task<Book> GetBook(int id) 
		{
			var book = await _context.Books
				.Include(b => b.Authors)
				.FirstOrDefaultAsync(b => b.BookId == id);
				
			if (book == null)
			{
				throw new KeyNotFoundException($"Book with ID {id} not found");
			}

			return book;
		}

		public async Task<Book> UpdateBook(int id, Book book)
		{
			if (id != book.BookId)
			{
				throw new ArgumentException("Book Id not found");
			}

			_context.Entry(book).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if(!BookExists(id))
				{
					throw new KeyNotFoundException($"Book with ID {id} not found");
				}
				else
				{
					throw;
				}
			}
			return book;
		}

		public async Task<Book> AddBook(CreateBookDTO bookDto)
		{
			if (bookDto.ReleaseYear < 1500 || bookDto.ReleaseYear > DateTime.Now.Year)
			{
				throw new ArgumentException($"ReleaseYear must be between 1500 and {DateTime.Now.Year}");
			}

			var authors = await _context.Authors
				.Where(a => bookDto.AuthorsIds.Contains(a.AuthorId))
				.ToListAsync();

			var foundAuthorIds = authors.Select(a => a.AuthorId).ToList();
			var missingAuthorIds = bookDto.AuthorsIds.Except(foundAuthorIds).ToList();

			if (missingAuthorIds.Any())
			{
				throw new ArgumentException($"Theese Author Id's do not exist: {string.Join(", ", missingAuthorIds)}");
			}

			var book = new Book()
			{
				Title = bookDto.Title,
				ISBN = bookDto.ISBN,
				Rating = bookDto.Rating,
				ReleaseYear = bookDto.ReleaseYear,
				Authors = authors
			};

			_context.Books.Add(book);
			await _context.SaveChangesAsync();
			return book;
		}

		public async Task<bool> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);

			if (book == null)
			{
				return false;
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();
			return true;
		}
		private bool BookExists(int id)
		{
			return _context.Books.Any(e => e.BookId == id);
		}
	}
}
