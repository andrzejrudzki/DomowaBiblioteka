using DomowaBiblioteka.Common.Books;
using DomowaBiblioteka.SQL.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomowaBiblioteka.SQL.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext context;

        public SQLBookRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Book Add(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
            return book;
        }

        public Book Delete(int Id)
        {
            Book employee = context.Books.Find(Id);
            if (employee != null)
            {
                context.Books.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Book> GetAll()
        {
            return context.Books;
        }

        public Book Get(int Id)
        {
            return context.Books.Find(Id);
        }

        public Book Update(Book bookChanges)
        {
            var employee = context.Books.Attach(bookChanges);
            employee.State = EntityState.Modified;
            context.SaveChanges();
            return bookChanges;
        }
    }
}
