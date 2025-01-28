namespace BooksDb.Models
{
    public class BookLoan
    {
        public int BookLoanId { get; set; }
        public int CopyId { get; set; }
        public int BorrowerId { get; set; }
        public DateOnly LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; }

        public Borrower? Borrower { get; set; } 
        public Copy? Copy { get; set; }
	}
}
