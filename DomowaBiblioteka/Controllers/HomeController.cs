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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DomowaBiblioteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBookRepository _bookRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IBookRepository bookRepository, IHostingEnvironment hostingEnvironment)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _bookRepository.GetAll();
            return View(model);
        }

        public IActionResult Details(int? id)
        {
            Book book = _bookRepository.Get(id??1);
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Id = book.Id,
                Cover = book.Cover,
                Date = book.Date,
                ItemType = book.ItemType,
                Title = book.Title,
                AuthorName = book.AuthorName,
                Status = book.Status,
            };   
            
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Book newBook = new Book
                {
                    Title = model.Title,
                    Date = model.Date,
                    ItemType = model.ItemType,
                    Cover = uniqueFileName,
                    Status = model.Status
                };

                _bookRepository.Add(newBook);
                return RedirectToAction("Details", new { id = newBook.Id });
               }

            return View();
        }


        [HttpGet]
        public ViewResult Edit(int id)
        {
            Book book = _bookRepository.Get(id);
            BookEditViewModel bookEditViewModel = new BookEditViewModel
            {
                Id = book.Id,
                Date = book.Date,
                ItemType = book.ItemType,
                Title = book.Title,
                AuthorName = book.AuthorName,
                ExistingPhotoPath = book.Cover,
                Status = book.Status
            };
            return View(bookEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(BookEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = _bookRepository.Get(model.Id);
                book.Title = model.Title;
                book.Date = model.Date;
                book.ItemType = model.ItemType;
                book.AuthorName = model.AuthorName;
                book.Status = model.Status;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "files", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    book.Cover  = ProcessUploadedFile(model);
                }              
                _bookRepository.Update(book);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(CreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
