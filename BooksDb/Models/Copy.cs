namespace BooksDb.Models
{
	public class Copy
	{
		public int CopyId { get; set; }
		public int BookId { get; set; }
		public bool IsAvailable { get; set; }
		public Book? Book { get; set; }
		public List<BookLoan> BookLoans { get; set; } = new();
	}
}
	