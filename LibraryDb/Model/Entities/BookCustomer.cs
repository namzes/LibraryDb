namespace LibraryDb.Model.Entities
{
	public class BookCustomer
	{
		public int Id { get; set; }
		public int BookId { get;set; }
		public Book? Book { get; set; }
		public int CustomerId { get; set; }
		public Customer? Customer { get; set; }
	}
}
