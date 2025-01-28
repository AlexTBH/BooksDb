namespace BooksDb.Models
{
	public class Author
	{
		public int AuthorId { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public List<Book> Books { get; set; } = new();
	}
}
