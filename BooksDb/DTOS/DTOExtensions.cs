using BooksDb.Models;
using BooksDb.DTOS;
using Microsoft.EntityFrameworkCore;
using CreateBooksDb.DTOS;

namespace BooksDb.DTOS
{
	public static class DTOExtensions
	{

		public static Borrower ToBorrower(this CreateBorrowerDTO borrowerDto)
		{
			var borrower = new Borrower()
			{
				FirstName = borrowerDto.FirstName,
				LastName = borrowerDto.LastName,
			};

			return borrower;
		}

		public static ToBorrowerDTO ToBorrowerDTO(this Borrower borrower)
		{
			var borrowerDto = new ToBorrowerDTO()
			{
				FirstName = borrower.FirstName,
				LastName = borrower.LastName,
				LoanCard = borrower.LoanCard,
			};

			return borrowerDto;
		}
		public static Copy ToCopy(this CreateCopyDTO copyDto)
		{

			Copy copy = new Copy()
			{
				BookId = copyDto.BookId,
				IsAvailable = true
			};
			return copy;
		}

		public static Author ToAuthor(this CreateAuthorDTO authorDTO)
		{
			var author = new Author()
			{
				FirstName = authorDTO.FirstName,
				LastName = authorDTO.LastName
			};
			return author;
		}

		public static ToAuthorDTO ToAuthorDTO(this Author author)
		{
			var authorDTO = new ToAuthorDTO
			{
				FirstName = author.FirstName,
				LastName = author.LastName
			};

			return authorDTO;
		}
		public static ToBookDTO ToBookDTO(this Book book)
		{
			var bookDto = new ToBookDTO 
			{
			BookId = book.BookId,
			Title = book.Title,
			ISBN = book.ISBN,
			Rating = book.Rating,
			ReleaseYear = book.ReleaseYear,
			AuthorsNames = book.Authors.Select(a => a.FirstName + " " + a.LastName).ToList()
			};

			return bookDto;
		}
	}
}
