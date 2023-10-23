using System.Collections.ObjectModel;
using CSA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement;

public sealed class DatabaseManagementContext : DbContext
{
    private readonly Random _rnd = new();
    
    public DatabaseManagementContext(DbContextOptions<DatabaseManagementContext> options) : base(options)
    {
        //this.Database.EnsureDeleted();
        if (!this.Database.EnsureCreated()) return;
        
        if(this.Database.IsNpgsql())
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var card = new Card
        {
            CardNumber = GetCardNumber(),
            Balance = 0m,
        };
        this.Cards.Add(card);

        var user = new User
        {
            Login = "Login",
            Password = "Password"
        };
        user.Cards.Add(card);
        this.Users.Add(user);

        var product = new Product
        {
            Name = "Item 1",
            Description = "Item 1",
            DateCreated = DateTime.Now,
            Price = 25m,
            Weight = 1m
        };
        this.Products.Add(product);

        var transaction = new Transaction
        {
            Card = card,
            DateCreated = DateTime.Now,
            ProductTransactions = new ObservableCollection<ProductTransaction>
            {
                new()
                {
                    Products = product,
                    Quantity = 5
                }
            },
            TransactionType = TransactionType.Sale
        };
        this.Transactions.Add(transaction);
        
        this.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.DateCreated)
            .HasColumnType("timestamp");
        
        modelBuilder.Entity<User>()
            .Property(p => p.DateCreated)
            .HasColumnType("timestamp");
        
        modelBuilder.Entity<Transaction>()
            .Property(p => p.DateCreated)
            .HasColumnType("timestamp");
    }

    public required DbSet<Product> Products { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Card> Cards { get; set; }
    public required DbSet<ProductTransaction> ProductTransactions { get; set; }
    public required DbSet<Transaction> Transactions { get; set; }

    private string GetCardNumber()
    {
        var card = "";
        for (var i = 0; i < 10; i++)
        {
            card += _rnd.Next(0, 10);
        }

        return card;
    }
}