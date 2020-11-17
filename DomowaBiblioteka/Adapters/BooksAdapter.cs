using DomowaBiblioteka.Models;

namespace DomowaBiblioteka.Adapters
{
    public static class BooksAdapter
    {
        public static Book ConvertFromDto(DomowaBiblioteka.Common.Books.Book bookDto)
        {
            return new Book
            {
                AuthorName = bookDto.AuthorName,
                Cover = bookDto.Cover,
                Date = bookDto.Date,
                Id = bookDto.Id,
                ItemType = bookDto.ItemType,
                Status = bookDto.Status,
                Title = bookDto.Title
            };
        }

        public static DomowaBiblioteka.Common.Books.Book ConvertToDto(Book book)
        {
            return new DomowaBiblioteka.Common.Books.Book
            {
                AuthorName = book.AuthorName,
                Cover = book.Cover,
                Date = book.Date,
                Id = book.Id,
                ItemType = book.ItemType,
                Status = book.Status,
                Title = book.Title
            };
        }
    }
}
