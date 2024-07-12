using ATMAPPAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMAPPAPI.Contexts
{
    public class AtmDbContext : DbContext
    {
        public AtmDbContext(DbContextOptions<AtmDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CardInfo> CardInfos { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User to CardInfo one-to-one relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.CardInfo)
                .WithOne(ci => ci.User)
                .HasForeignKey<CardInfo>(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to UserCredentials one-to-one relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Credentials)
                .WithOne(uc => uc.User)
                .HasForeignKey<UserCredentials>(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User to Account one-to-one relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<Account>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account to Transactions one-to-many relationship
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // CardInfo validation
            modelBuilder.Entity<CardInfo>()
                .Property(ci => ci.CardNumber)
                .IsRequired()
                .HasMaxLength(16);

            modelBuilder.Entity<CardInfo>()
                .Property(ci => ci.CVV)
                .IsRequired()
                .HasMaxLength(3);

            modelBuilder.Entity<CardInfo>()
                .Property(ci => ci.ExpiryDate)
                .IsRequired();

            // UserCredentials validation
            modelBuilder.Entity<UserCredentials>()
                .Property(uc => uc.Pin)
                .IsRequired()
                .HasMaxLength(4);

            modelBuilder.Entity<UserCredentials>()
                .Property(uc => uc.Otp)
                .HasMaxLength(6);

            // User validation
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            // Account validation
            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Transaction validation
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
