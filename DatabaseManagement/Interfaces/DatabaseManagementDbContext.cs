using DatabaseManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Interfaces;

public abstract class DatabaseManagementDbContext : DbContext
{
	protected DatabaseManagementDbContext(DbContextOptions<DatabaseManagementDbContext> options) : base(options)
	{
	}

	public required DbSet<Product> Products { get; set; }
}
