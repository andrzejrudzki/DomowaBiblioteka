using DomowaBiblioteka.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DomowaBiblioteka.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAll();
        }

        [HttpGet("{id}")]
        public Book Get(int id)
        {
            return _bookRepository.Get(id);
        }

        [HttpPost]
        public Book Post([FromBody] Book value)
        {
            return _bookRepository.Add(value);
        }

        [HttpPut]
        public void Put([FromBody] Book value)
        {
            _bookRepository.Update(value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _bookRepository.Delete(id);
        }
    }
}