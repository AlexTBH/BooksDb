using System.ComponentModel.DataAnnotations;

namespace BooksDb.Models
{
	public class Book
	{
		public int BookId { get; set; }
		public required string Title { get; set; }
		public required string ISBN { get; set; }

		[Range(0, 5)]
		public int Rating { get; set; }
		public int ReleaseYear { get; set; }
		public List<Author> Authors { get; set; } = new();
		
		
	}
}
