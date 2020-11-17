using DomowaBiblioteka.Common.Books;
using DomowaBiblioteka.SQL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<BookController>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAll();
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public Book Get(int id)
        {
            return _bookRepository.Get(id);
        }

        // POST api/<BookController>
        [HttpPost]
        public Book Post([FromBody]Book value)
        {
            return _bookRepository.Add(value);
        }

        // PUT api/<BookController>/5
        [HttpPut()]
        public void Put([FromBody]Book value)
        {
            _bookRepository.Update(value);
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _bookRepository.Delete(id);
        }
    }
}
