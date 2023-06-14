using System;
using Microsoft.EntityFrameworkCore;
using myDadApp.Models;

namespace myDadApp.Data
{
	public class myDataContext : DbContext
	{
		public DbSet<myDadApp.Models.Chore> Chore { get; set; }

		public myDataContext()
		{
		}
		public myDataContext(DbContextOptions<myDataContext> options) : base(options)
		{
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("A fallback connecion string");
			}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<myDadApp.Models.Owner> Owner { get; set; }

    }
}