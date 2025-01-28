using BooksDb.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksDb.Services
{
	public class BorrowerService
	{
		private readonly AppDbContext _context;
		private readonly BookLoanService _bookLoanService;

		public BorrowerService(AppDbContext context, BookLoanService bookLoanService)
		{
			_context = context;
			_bookLoanService = bookLoanService;
		}

		public async Task<IEnumerable<Borrower>> GetBorrowers()
		{
			return await _context.Borrowers.ToListAsync();
		}
		public async Task<Borrower> GetBorrower(int id) 
		{
			var borrower = await _context.Borrowers.FindAsync(id);

			if (borrower == null)
			{
				throw new KeyNotFoundException($"Borrower with Id {id} not found");
			}
			
			return borrower;
		}

		public async Task<Borrower> UpdateBorrower(int id, Borrower borrower)
		{
			if (id != borrower.BorrowerId)
			{
				throw new ArgumentException("Borrower ID not found");
			}

			_context.Entry(borrower).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BorrowerExists(id))
				{
					throw new KeyNotFoundException($"Borrower with ID {id} not found");
				}
				else
				{
					throw;
				}
			}
			return borrower;
		}

		public async Task<Borrower> AddBorrower(Borrower borrower)
		{
			_context.Borrowers.Add(borrower);
			await _context.SaveChangesAsync();
			return borrower;
		}
		public async Task<bool> DeleteBorrower(int id)
		{
			var borrower = await _context.Borrowers.FindAsync(id);

			if (borrower == null)
			{
				return false;
			}

			var loans = await _bookLoanService.GetLoansByBorrower(borrower.LoanCard);

			
			if (loans.Any())
			{
				foreach (var loan in loans)
				{
					await _bookLoanService.ReturnLoan(loan.BookLoanId);
				}
			}

			_context.Borrowers.Remove(borrower);
			await _context.SaveChangesAsync();

			return true;
		}
		private bool BorrowerExists(int id)
		{
			return _context.Borrowers.Any(e => e.BorrowerId == id);
		}
	}
}
