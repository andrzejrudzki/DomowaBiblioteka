using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
{

    
    public class MockBookRepository:IBookRepository
    {
        private List<Book>  _bookList ;

        public MockBookRepository()
        {
            _bookList = new List<Book>()
            {
                new Book(){Title="Book1",Cover="string", AuthorName=""},
                new Book(){Title="Book2",Cover="string", AuthorName=""},
                new Book(){Title="Book3",Cover="string", AuthorName=""},
                new Book(){Title="Book4",Cover="string", AuthorName=""}
            };
        }

        public Book Add(Book book)
        {
            _bookList.Add(book);
            return book;
        }

        public Book Delete(int id)
        {
            Book book = _bookList.FirstOrDefault(e => e.Id == id);
            if (book != null)
            {
                _bookList.Remove(book);
            }
            return book;
        }

        public Book Get(int Id)
        {
            return _bookList[Id];
        }

        public IEnumerable<Book> GetAll()
        {
            return _bookList;
        }

        public Book Update(Book bookChanges)
        {
            Book employee = _bookList.FirstOrDefault(e => e.Id == bookChanges.Id);
            if (employee != null)
            {
                employee.Cover = bookChanges.Cover;
                employee.Date = bookChanges.Date;
                employee.AuthorName = bookChanges.AuthorName;
                employee.ItemType = bookChanges.ItemType;
            }
            return employee;
        }
    }
}
