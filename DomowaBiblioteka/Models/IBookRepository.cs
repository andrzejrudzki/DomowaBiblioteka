using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
{
    public interface IBookRepository
    {
        Book Add(Book book);
        List<Book> GetAll(); 
    }
}
