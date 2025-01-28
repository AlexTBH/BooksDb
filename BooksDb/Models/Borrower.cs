namespace BooksDb.Models
{
	public class Borrower
	{
		public int BorrowerId { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public int LoanCard {  get; set; }
		public List<BookLoan> BookLoans { get; set; } = new();
	}
}
