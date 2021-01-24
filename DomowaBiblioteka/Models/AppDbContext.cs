using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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



