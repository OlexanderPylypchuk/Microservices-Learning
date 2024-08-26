
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Models;
namespace ShoppingCartAPI.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
        public DbSet<CartHeader> Headers { get; set; }
		public DbSet<CartDetails> Details { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
