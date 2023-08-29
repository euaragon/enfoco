using System;
using Microsoft.EntityFrameworkCore;
using enfoco2.Models;
namespace enfoco2.Data
{
	public class EnfocoDb : DbContext
	{
		public EnfocoDb(DbContextOptions<EnfocoDb> options) : base(options)
		{

		}
		public DbSet<Notice> Notices => Set<Notice>();
	}
}

