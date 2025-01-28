namespace BooksDb.DTOS
{
	public class BorrowerLoanDto
	{
		public int BookLoanId { get; set; }
		public required string BookName { get; set; }
		public required string BorrowerName { get; set; }
		public DateOnly LoanDate { get; set; }
		public DateOnly? ReturnDate { get; set; }
	}
}
