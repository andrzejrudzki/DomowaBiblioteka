using DomowaBiblioteka.Common.Books;
using Microsoft.EntityFrameworkCore;
using static DomowaBiblioteka.Common.Enums.Enums;

namespace DomowaBiblioteka.SQL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
             new Book
             {
                 Id = 1,
                 ItemType = ItemType.Book,
                 Title = "Book-Title",
                 AuthorName = "Author1"
             },
             new Book
             {
                 Id = 2,
                 ItemType = ItemType.CD,
                 Title = "CD-Title",
                 AuthorName = "Author2"
             },
             new Book
             {
                 Id = 3,
                 ItemType = ItemType.DVD,
                 Title = "DVD-Title",
                 AuthorName = "Author3"
             },
             new Book
             {
                 Id = 4,
                 ItemType = ItemType.Book,
                 Title = "Book2-Title",
                 AuthorName = "Author4"
             }
         );

        }
    }
}
