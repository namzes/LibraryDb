namespace LibraryDb.Model.Entities
{
	public class BookCustomer
	{
		public int BookId { get;set; }
		public int CustomerId { get; set; }
		public Book? Book { get; set; }
		public Customer? Customer { get; set; }
	}
}
