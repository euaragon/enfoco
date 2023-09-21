using System;
namespace enfoco2.Models
{
	public class Newsletter
	{
        public int Id { get; set; }

        public required string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class NewsletterDto : Newsletter
    {
    }
}

