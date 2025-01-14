using LibraryDb.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryDb.Model.LibraryContext
{
	public class LibraryContext : DbContext
	{
		public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
		{

		}

		public DbSet<Book> Books { get; set; } = null!;
		public DbSet<Customer> Customers { get; set; } = null!;
		public DbSet<BookCustomer> BooksCustomers { get; set; } = null!;
		public DbSet<Author> Authors { get; set; } = null!;
		public DbSet<BookInfoAuthor> BookInfoAuthors { get; set; } = null!;
		public DbSet<BookInfo> BookInfos { get; set; } = null!;
		public DbSet<Loan> Loans { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BookInfo>()
				.HasMany(bi => bi.BookInfoAuthors)
				.WithOne(bia => bia.BookInfo)
				.HasForeignKey(bia => bia.Id)
				.IsRequired(false);

			modelBuilder.Entity<Author>()
				.HasMany(a => a.BookInfoAuthors)
				.WithOne(bia => bia.Author)
				.HasForeignKey(bia => bia.Id)
				.IsRequired(false);
		}
	}
}
