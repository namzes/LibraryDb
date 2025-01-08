using Microsoft.Build.Framework;

namespace LibraryDb.Model.Entities
{
	public class BookCustomer
	{
		public int Id { get; set; }
		[Required]
		public required Book Book { get; set; }
		[Required]
		public required Customer Customer { get; set; }
	}
}
