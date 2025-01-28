using BooksDb.Models;
using CreateBooksDb.DTOS;

namespace BooksDb.DTOS
{
	public class ToBookDTO
	{
		public int BookId { get; set; }
		public string Title { get; set; } = null!;
		public string ISBN { get; set; } = null!;
		public int Rating { get; set; }
		public int ReleaseYear { get; set; }
		public List<string> AuthorsNames { get; set; } = new();
	}

}
