namespace LibraryDb.Model.Entities
{
	public class BookInfoAuthor
	{
		public int Id { get; set; }
		public required BookInfo BookInfo { get; set; }
		public required Author Author { get; set; }

	}
}
