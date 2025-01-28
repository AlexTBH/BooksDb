using BooksDb.Controllers;
using BooksDb.DTOS;
using BooksDb.Models;
using Microsoft.EntityFrameworkCore;


namespace CreateBooksDb.DTOS
{
	public class CreateBookDTO
	{
		public string Title { get; set; } = null!;
		public string ISBN { get; set; } = null!;
		public int Rating { get; set; }
		public int ReleaseYear { get; set; }
		public List<int> AuthorsIds { get; set; } = new();

		public CreateBookDTO(string title, string iSBN, int rating, int releaseYear, List<int> authorsIds)
		{
			Title = title;
			ISBN = iSBN;
			Rating = rating;
			ReleaseYear = releaseYear;
			AuthorsIds = authorsIds;

		}
	}
}

