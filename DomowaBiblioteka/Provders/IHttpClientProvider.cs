using DomowaBiblioteka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Provders
{
    public interface IHttpClientProvider
    {
        Task<Book> Get(int id);
        Task<IEnumerable<Book>> GetAll();
        Task<Book> Add(Book book);
        Task Update(Book bookChanges);
        Task Delete(int id);
    }
}
