using BooksDb.Models;

namespace BooksDb.DTOS
{
	public class CreateBorrowerDTO
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public CreateBorrowerDTO(string firstName, string lastName)
		{
			FirstName = firstName;
			LastName = lastName;
		}
	}
}
