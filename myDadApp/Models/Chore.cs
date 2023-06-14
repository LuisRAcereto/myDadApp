using System;
namespace myDadApp.Models
{
	public class Chore
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Owner { get; set; }
		public bool IsDone { get; set; }
		public DateTime CreateDt { get; set; }
		public DateTime? CompleteDt { get; set; }

		public Chore()
		{
			Owner = "Demo";
			Id = Guid.NewGuid().ToString();
			CreateDt = DateTime.UtcNow;
			IsDone = false;
		}
	}
}

