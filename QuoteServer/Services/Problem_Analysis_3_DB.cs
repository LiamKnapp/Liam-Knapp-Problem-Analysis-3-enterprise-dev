using Microsoft.EntityFrameworkCore;
using Azure;
using QuoteAppServer.Entities;

namespace QuoteAppServer.Services
{
    public class QuotesDB : DbContext
    {
        public QuotesDB(DbContextOptions<QuotesDB> options) : base(options)
        {
        }

        public DbSet<Quotes> Quotes { get; set; }
        public DbSet<QuoteTags> QuoteTags { get; set; }
        public DbSet<QuoteTagJunction> QuoteTagJunctions { get; set; } // Add this DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quotes>()
                .HasMany(q => q.QuoteTags)
                .WithMany(qt => qt.Quote)
                .UsingEntity<QuoteTagJunction>(
            j => j
                .HasOne(qt => qt.Tag)
                .WithMany()
                .HasForeignKey(qt => qt.TagId),
            j => j
                .HasOne(qt => qt.Quote)
                .WithMany()
                .HasForeignKey(qt => qt.QuoteId),
            j =>
            {
                j.HasKey(qt => new { qt.QuoteId, qt.TagId });
                j.ToTable("QuoteWithQuoteTags");
            }
        );

            base.OnModelCreating(modelBuilder);


            // Seeding quotes:
            modelBuilder.Entity<Quotes>().HasData(
                new Quotes { QuoteId = 1, Quote = "To be, or not to be: that is the question", Author = "William Shakespeare", Likes = 0 },
                new Quotes { QuoteId = 2, Quote = "I have a dream", Author = "Martin Luther King Jr.", Likes = 2 },
                new Quotes { QuoteId = 3, Quote = "The only thing we have to fear is fear itself", Author = "Franklin D. Roosevelt", Likes = 10 }
            );

            // Seeding tags:
            modelBuilder.Entity<QuoteTags>().HasData(
                new QuoteTags() { TagId = 1, TagName = "Wise" },
                new QuoteTags() { TagId = 2, TagName = "Inspiring" },
                new QuoteTags() { TagId = 3, TagName = "Brave" }
            );

            // Seeding junction table entries (quotes with tags):
            modelBuilder.Entity<QuoteTagJunction>().HasData(
                new QuoteTagJunction { QuoteId = 1, TagId = 1 },
                new QuoteTagJunction { QuoteId = 2, TagId = 2 },
                new QuoteTagJunction { QuoteId = 3, TagId = 3 },
                new QuoteTagJunction { QuoteId = 1, TagId = 2 }
            );
        }
    }
}
