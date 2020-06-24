using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
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

