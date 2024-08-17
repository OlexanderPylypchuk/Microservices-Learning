using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ProductAPI.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>().HasData([
				new Product() {
					Id=1,
					Name = "Samosa",
					Price = 15,
					CategoryName = "Apetizer",
					Description = "yes.",
					ImgUrl = ""
				},
				new Product()
				{
					Id = 2,
                    Name = "Salsa",
                    Price = 20,
                    CategoryName = "Apetizer",
                    Description = "yes.",
                    ImgUrl = "" 
				}
				]);
		}
	}
}
