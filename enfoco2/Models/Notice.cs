using System;
namespace enfoco2.Models
{
	public class Notice
	{
		public int Id { get; set; }

		public required string Title { get; set; }

		public required string Subtitle { get; set; }

		public required string Issue { get; set; }

		public required string Text { get; set; }

		public required string Img { get; set; }
	}
    public class NoticeDto : Notice
    {
    }

}

