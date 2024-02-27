using Microsoft.EntityFrameworkCore;

namespace Sandbox.ConsoleApp;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {

    }
    public DbSet<TestItem>? Items { get; set; }
}

public class TestItem
{
    public int Id { get; set; }
    public required string Value { get; set; }
}