namespace BooksDb.DTOS
{
	public class ToBorrowerDTO
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public int LoanCard { get; set; }

	}
}
