using DomowaBiblioteka.Common.Books;
using System.Collections.Generic;
using System.Linq;
using static DomowaBiblioteka.Common.Enums.Enums;

namespace DomowaBiblioteka.SQL.Repositories
{
    public class MockBookRepository : IBookRepository
    {
        private List<Book> _bookList;

        public MockBookRepository()
        {
            _bookList = new List<Book>()
            {
                new Book(){Id = 1,ItemType=ItemType.Book, Title="Book-Title", AuthorName="Author1"},
                new Book(){Id = 2,ItemType=ItemType.CD,  Title="CD-Title", AuthorName="Author2"},
                new Book(){Id = 3,ItemType=ItemType.DVD,  Title="DVD-Title", AuthorName="Author3"},
                new Book(){Id = 4,ItemType=ItemType.Book,  Title="Book2-Title", AuthorName="Author4"}
            };
        }

        public Book Add(Book book)
        {
            book.Id = _bookList.Max(e => e.Id) + 1;
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
            return _bookList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _bookList;
        }

        public Book Update(Book bookChanges)
        {
            Book book = _bookList.FirstOrDefault(e => e.Id == bookChanges.Id);
            if (book != null)
            {
                book.Title = bookChanges.Title;
                book.Cover = bookChanges.Cover;
                book.Date = bookChanges.Date;
                book.AuthorName = bookChanges.AuthorName;
                book.ItemType = bookChanges.ItemType;
                book.Status = bookChanges.Status;
            }
            return book;
        }
    }
}
