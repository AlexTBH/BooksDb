using BooksDb.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksDb.DTOS
{
	public class CreateBookLoanDTO
	{
		public int CopyId { get; set; }
		public int BorrowerId { get; set; }

		public CreateBookLoanDTO(int copyId, int borrowerId)
		{
			CopyId = copyId;
			BorrowerId = borrowerId;
			
		}
	}
}
