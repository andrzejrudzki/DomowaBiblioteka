using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DomowaBiblioteka.Models;
using System.Diagnostics.Eventing.Reader;
using DomowaBiblioteka.ViewModels;
using Microsoft.CodeAnalysis.Operations;

namespace DomowaBiblioteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBookRepository _bookRepository;

        public HomeController(ILogger<HomeController> logger, IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public ViewResult Index()
        {
            var model = _bookRepository.GetAll();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {

            Book model = _bookRepository.Get(id??1);
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Id = model.Id,
                Cover = model.Cover,
                Date = model.Date,
                ItemType = model.ItemType,
                Title = model.Title,
                AuthorName = model.AuthorName,
            };
            
            
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(Book model)
        {
            if (ModelState.IsValid)
            {
            Book book = _bookRepository.Add(model);
            return RedirectToAction("Details", new { id = book.Id });
            }
            return View();
       }



        public IActionResult BooksList()
        {
            return View(_bookRepository.GetAll());
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
