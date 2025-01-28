using BooksDb.DTOS;
using BooksDb.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksDb.Services
{
    public class BookLoanService
    {
        private readonly AppDbContext _context;

        public BookLoanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookLoan>> GetBookLoans()
        {
            return await _context.BookLoans.ToListAsync();
        }
        public async Task<BookLoan> GetBookLoan(int id)
        {
            var bookLoan = await _context.BookLoans.FindAsync(id);

            if (bookLoan == null)
            {
                throw new KeyNotFoundException($"Bookloan with ID {id} not found");
            }
            return bookLoan;
        }

        public async Task<BookLoan> UpdateBookLoan(int id, BookLoan bookLoan)
        {
            if (id != bookLoan.BookLoanId)
            {
                throw new ArgumentException("Bookloan ID not found");
            }

            _context.Entry(bookLoan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookLoanExists(id))
                {
                    throw new KeyNotFoundException($"Bookloan with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }
            return bookLoan;
        }

        public async Task<IEnumerable<BorrowerLoanDto>> GetLoansByBorrower(int borrowerLoanCard)
        {

            var loans = await _context.BookLoans
                .Include(bl => bl.Copy!)
                    .ThenInclude(copy => copy.Book)
                .Include(bl => bl.Borrower)
                .Where(bl => bl.Borrower != null && bl.Borrower.LoanCard == borrowerLoanCard && bl.ReturnDate == null)
                .ToListAsync();

            if (!loans.Any())
            {
                return Enumerable.Empty<BorrowerLoanDto>();
            }

			var loanDtos = loans.Select(bl => new BorrowerLoanDto
			{
				BookLoanId = bl.BookLoanId,
				BookName = bl.Copy?.Book?.Title ?? "Unknown book",
				BorrowerName = bl.Borrower != null ? $"{bl.Borrower.FirstName} {bl.Borrower.LastName}" : "Unknown borrower",
				LoanDate = bl.LoanDate,
				ReturnDate = bl.ReturnDate
			});

			return loanDtos;
        }

        public async Task ReturnLoan(int bookLoanId)
        {
            var loan = await _context.BookLoans
                .Include(bl => bl.Copy)
                .FirstOrDefaultAsync(bl => bl.BookLoanId == bookLoanId);


            if (loan == null)
            {
                throw new KeyNotFoundException($"Loan with ID {bookLoanId} not found");
            }

            if (loan.Copy == null)
            {
                throw new InvalidOperationException($"Loan with ID {bookLoanId} has no associated copy");
            }

            loan.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            loan.Copy.IsAvailable = true;

            await _context.SaveChangesAsync();
        }

        public async Task<BookLoan> AddBookLoan(CreateBookLoanDTO bookLoanDTO)
        {
            var copy = await _context.Copies
                .FirstOrDefaultAsync(c => c.CopyId == bookLoanDTO.CopyId);

            var borrower = await _context.Borrowers
                .FirstOrDefaultAsync(b => b.BorrowerId == bookLoanDTO.BorrowerId);

            if (copy == null)
            {
                throw new KeyNotFoundException($"Copy with ID {bookLoanDTO.CopyId} not found");
            }

            if (!copy.IsAvailable)
            {
                throw new InvalidOperationException("The book is not available for loan");
            }

            if (borrower == null)
            {
                throw new KeyNotFoundException($"Borrower with ID {bookLoanDTO.BorrowerId} not found");
            }

            var bookLoan = new BookLoan()
            {
                CopyId = bookLoanDTO.CopyId,
                BorrowerId = bookLoanDTO.BorrowerId,
                LoanDate = DateOnly.FromDateTime(DateTime.Now)
            };

            copy.IsAvailable = false;

            _context.BookLoans.Add(bookLoan);
            await _context.SaveChangesAsync();

            return bookLoan;
        }

        public async Task<bool> DeleteBookLoan(int id)
        {
            var bookLoan = await _context.BookLoans.FindAsync(id);

            if (bookLoan == null)
            {
                return false;
            }

            _context.BookLoans.Remove(bookLoan);
            await _context.SaveChangesAsync();

            return true;

        }

        private bool BookLoanExists(int id)
        {
            return _context.BookLoans.Any(e => e.BookLoanId == id);
        }
    }
}
