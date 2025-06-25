using Microsoft.EntityFrameworkCore;
using NGOAutomationAPI.Models;

namespace NGOAutomationAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
           public DbSet<User> Users { get; set; }
	}
}
