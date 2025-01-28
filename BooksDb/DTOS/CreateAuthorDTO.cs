using BooksDb.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BooksDb.DTOS
{
	public class CreateAuthorDTO
	{
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;

		public CreateAuthorDTO(string firstName, string lastName) 
		{
			FirstName = firstName;
			LastName = lastName;
		}

	}
}
