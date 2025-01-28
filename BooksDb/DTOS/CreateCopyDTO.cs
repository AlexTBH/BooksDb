using BooksDb.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace BooksDb.DTOS
{
	public class CreateCopyDTO
	{	
 		public int BookId { get; set; }
		public CreateCopyDTO(int bookId) 
		{
			BookId = bookId;
		}

	}

}
