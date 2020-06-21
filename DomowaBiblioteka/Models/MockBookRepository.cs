using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
{

    
    public class MockBookRepository:IBookRepository
    {
        private List<Book>  _bookList = new List<Book>();

        public MockBookRepository()
        {
            _bookList.Add(new Book() { Title="Book1",Cover="string", AuthorName=""});
            _bookList.Add(new Book() { Title = "Book1", Cover = "string", AuthorName = "" });
            _bookList.Add(new Book() { Title = "Book1", Cover = "string", AuthorName = "" });
        }


        public Book Add(Book book)
        {
            _bookList.Add(book);
            return book;
        }

        public List<Book> GetAll()
        {
            return _bookList;
        }
    }
}
