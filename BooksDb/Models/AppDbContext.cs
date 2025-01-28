using Microsoft.EntityFrameworkCore;


namespace BooksDb.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) 
			: base(options) 
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Book>()
				.HasMany(b => b.Authors)
				.WithMany(a => a.Books)
				.UsingEntity<Dictionary<string, object>>(
					"AuthorBook",
					j => j.HasOne<Author>()
						.WithMany()
						.HasForeignKey("AuthorId"),
					j => j.HasOne<Book>()
						.WithMany()
						.HasForeignKey("BookId")
				)
				.ToTable("AuthorBook");

			modelBuilder.Entity<Book>()
				.HasIndex(b => b.ISBN)
				.IsUnique();

			modelBuilder.Entity<Copy>()
				.HasOne(c => c.Book)
				.WithMany()
				.HasForeignKey(c => c.BookId);

			modelBuilder.Entity<BookLoan>()
				.HasOne(bl => bl.Borrower)
				.WithMany(b => b.BookLoans)
				.HasForeignKey(bl => bl.BorrowerId);

			modelBuilder.Entity<BookLoan>()
				.HasOne(bl => bl.Copy)
				.WithMany(c => c.BookLoans)
				.HasForeignKey(bl => bl.CopyId);

			modelBuilder.HasSequence<int>("LoanCardSequence", schema: "dbo")
				.StartsAt(90000)
				.IncrementsBy(1);

			modelBuilder.Entity<Borrower>()
				.Property(b => b.LoanCard)
				.HasDefaultValueSql("NEXT VALUE FOR dbo.LoanCardSequence");
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Borrower> Borrowers { get; set; }
		public DbSet<BookLoan> BookLoans{ get; set; }
		public DbSet<Copy> Copies { get; set; }
	}
}
