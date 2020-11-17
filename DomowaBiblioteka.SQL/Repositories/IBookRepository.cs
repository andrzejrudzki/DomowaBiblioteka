using DomowaBiblioteka.Common.Books;
using System.Collections.Generic;

namespace DomowaBiblioteka.SQL.Repositories
{
    public interface IBookRepository
    {
        Book Get(int Id);
        IEnumerable<Book> GetAll();
        Book Add(Book book);
        Book Update(Book bookChanges);
        Book Delete(int id);
    }
}
