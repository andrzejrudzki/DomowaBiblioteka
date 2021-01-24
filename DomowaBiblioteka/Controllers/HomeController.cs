using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using DomowaBiblioteka.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using DomowaBiblioteka.ViewModels;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using static DomowaBiblioteka.Models.Enums;
using Microsoft.CodeAnalysis.Operations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DomowaBiblioteka.Controllers


{
  
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, IBookRepository bookRepository, IHostingEnvironment hostingEnvironment)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            this.hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _bookRepository.GetAll();
            return View(model);
        }

        //----------------------------- Search -----------------------------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SearchResults", new
                {
                    AuthorName = model.AuthorName,
                    title = model.Title,
                    ItemType = model.ItemType.ToString()
                });
            }

            return View();
        }

        [AllowAnonymous]
        public ViewResult SearchResults(string AuthorName, string title, string ItemType)
        {
            var books = _bookRepository.GetAll();
            if (AuthorName != null)
            {
                books = books.Where(f => f.AuthorName == AuthorName);
            }
            if (title != null)
            {
                books = books.Where(f => f.Title == title);
            }

            if (ItemType != Enums.ItemTypeSearch.Wybierz.ToString())
            {
                books = books.Where(f => f.ItemType.ToString() == ItemType);
            }
            return View(books);
        }

        //----------------------------- Details -----------------------------

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            Book book = _bookRepository.Get(id ?? 1);
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Id = book.Id,
                Cover = book.Cover,
                Date = book.Date,
                ItemType = book.ItemType,
                Title = book.Title,
                AuthorName = book.AuthorName,
                Status = book.Status,
                DateOfRent = book.DateOfRent,
                RentalApprovingPerson = book.RentalApprovingPerson,
                Borrower = book.Borrower
            };
            return View(homeDetailsViewModel);
        }

        //----------------------------- Delete -----------------------------
        
        public async Task<IActionResult> DeleteBook(int id)
        {      
            _bookRepository.Delete(id);        
            var okladkadous = _bookRepository.Get(id);
            return RedirectToAction("Index");
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "filescontainers";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);  
            var blob = cloudBlobContainer.GetBlobReference(okladkadous.Cover);
            await blob.DeleteIfExistsAsync();
                                 
        }


      

        //----------------------------- Rent -----------------------------
        [HttpGet]
        public IActionResult Rent(int id)
        {
            RentViewModel rentViewModel = new RentViewModel()
            {
                Id = id,
            };
            return View(rentViewModel);
        }

        [HttpPost]
        public IActionResult Rent(RentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = _bookRepository.Get(model.Id);
                book.Borrower = model.Borrower;
                book.RentalApprovingPerson = User.Identity.Name;
                book.DateOfRent = DateTime.Now;
                book.Status = Enums.StatusType.Wypozyczony;
                _bookRepository.Update(book);

                return RedirectToAction("Details", new { id = model.Id });
            }
            return View();
        }

        public IActionResult Return(int id)
        {
            Book book = _bookRepository.Get(id);
            book.Status = Enums.StatusType.Dostepna;
            book.DateOfRent = DateTime.Now;
            book.RentalApprovingPerson = "";
            book.Borrower = "";

            _bookRepository.Update(book);
            return RedirectToAction("Details", new { id = id });
        }

        public ViewResult Statistics()
        {
            var books = _bookRepository.GetAll();

            StatisticsViewModel statisticsViewModel = new
            StatisticsViewModel()
            {
                elementsNumber = books.Count(),
                elementsNumberDVD = books.Where(f => f.ItemType == ItemType.DVD).Count(),
                elementsNumberCD = books.Where(f => f.ItemType == ItemType.CD).Count(),
                elementsNumberBook = books.Where(f => f.ItemType == ItemType.Book).Count(),

                elementsNumberAvailable = books.Where(f => f.Status == StatusType.Dostepna).Count(),
                elementsNumberUnavailable = books.Where(f => f.Status == StatusType.Wypozyczony).Count(),
            };

            List<UnavailableElement> unavailableElements = new List<UnavailableElement>();

            foreach (var element in books.Where(f => f.Status == StatusType.Wypozyczony))
            {
                UnavailableElement unavailableElement = new UnavailableElement
                {
                    DateOfRent = element.DateOfRent,
                    RentalApprovingPerson = element.RentalApprovingPerson,
                    Title = element.Title,
                };
                unavailableElements.Add(unavailableElement);
            }
            statisticsViewModel.unavailableElements = unavailableElements;



            return View(statisticsViewModel);
        }


        [HttpGet]
        public IActionResult Createblob()
        {
            return View();
        }
   
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile files, CreateViewModel model)

        {
            string nazwa_bloaba; 
            string nazwa_bloaba1;
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            byte[] dataFiles;

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);


            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filescontainers");


            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }


            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            string link = "~/files/noimage.jpg";
            string systemFileName;

            if (files != null)
            { 
                systemFileName = files.FileName;

                await using (var target = new MemoryStream())
                {
                    files.CopyTo(target);
                    dataFiles = target.ToArray();
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                nazwa_bloaba1 = cloudBlockBlob.Uri.ToString();

                nazwa_bloaba = files.FileName.ToString();  

            }
            else { nazwa_bloaba1 = "~/images/logo.png"; }
                                                        
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model); 

                Book newBook = new Book
                {
                    Title = model.Title,
                    Date = model.Date,
                    ItemType = model.ItemType,
                    AuthorName = model.AuthorName,

                    Cover = nazwa_bloaba1, 
                    Status = StatusType.Dostepna,
                    DateOfRent = DateTime.Now
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
            };
            return View(bookEditViewModel);
        }

        [HttpPost]
 
        public async Task<IActionResult> Edit(IFormFile files, BookEditViewModel model)
        {
           
            {
                int x = 0; 
                string nazwa_bloaba1; 
                string blobstorageconnection = _configuration.GetValue<string>
                ("blobstorage");
                byte[] dataFiles;

                CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(blobstorageconnection);

                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer =
                cloudBlobClient.GetContainerReference("filescontainers");

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new
                    BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                if (files != null)
                {
                    string systemFileName = files.FileName;
                    await using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        dataFiles = target.ToArray();

                    }

                    CloudBlockBlob cloudBlockBlob =
                    cloudBlobContainer.GetBlockBlobReference(systemFileName);
                    await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0,
                    dataFiles.Length);

                    nazwa_bloaba1 = cloudBlockBlob.Uri.ToString();
                    string nazwa_bloaba; 
                    nazwa_bloaba = files.FileName.ToString(); 
                }

                else
                {
                    nazwa_bloaba1 = "~/images/logo.png";
                    x = 1;

                }
               
                if (ModelState.IsValid)
                {
                    Book book = _bookRepository.Get(model.Id);
                    book.Title = model.Title;
                    book.Date = model.Date;
                    book.ItemType = model.ItemType;
                    book.AuthorName = model.AuthorName;

                    if (x == 0)
                    {
                        book.Cover = nazwa_bloaba1; // dorobilem1 pobiera adres uri bloba
                    }

                    _bookRepository.Update(book);
                    return RedirectToAction("index");
                }
                return View();
            }

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

