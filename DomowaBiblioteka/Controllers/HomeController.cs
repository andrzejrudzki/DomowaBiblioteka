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
// ---  dodane z HomeController ---
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using DomowaBiblioteka.ViewModels;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Diagnostics;

namespace DomowaBiblioteka.Controllers


{
    // zadani do testowania 
    //   tworzenie bloba  https://localhost:44367/blob/Createblob
    //   sprawdzanie blobow na dysku azurowym 

    public class HomeController : Controller
    {
        //----- dodane z homecontroler ---------
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        // --- ponizszy loger powoduje bledy ---
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, IBookRepository bookRepository, IHostingEnvironment hostingEnvironment)
        {          // chyba logowanie 
            _bookRepository = bookRepository;
            _logger = logger;
            this.hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        //------koniec z HomeController --------

        private readonly IConfiguration _configuration;
        //  public BlobController(IConfiguration configuration)
        //   {
        //      _configuration = configuration;
        //  }
        public ViewResult Index() // --- chyba wyswietlnie wszystkiego
        {
            var model = _bookRepository.GetAll();
            return View(model);
        }
        //-----------------------------
        public IActionResult Details(int? id) // z HomeControler
        {
            Book book = _bookRepository.Get(id ?? 1);
            HomeDetailsViewModel homeDetailsViewModel = new
            HomeDetailsViewModel()
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
        //=============================
        [HttpGet]
        // nazwa Createblob musi byc taka sama ja vidok Views/Blob/ Createblob.cs
        public IActionResult Createblob()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Createblob(IFormFile files)
        {


            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            byte[] dataFiles;
            // Retrieve storage account from connection string.  UseDevelopmentStorage=true;
            // tu jest blad
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);


            // Create the blob client.
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container.
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filescontainers");



            // --- tworzy kontener jezeli nie istnieje

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
            string systemFileName = files.FileName;
            await cloudBlobContainer.SetPermissionsAsync(permissions);
            await using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                dataFiles = target.ToArray();
                // files.FileName
            }
            // This also does not make a service call; it only creates a local object.
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
            await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

            return View();
        }



        //============================================================
        //............................................................


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // --- blob ---  public async Task<IActionResult> Createblob(IFormFile files)



        [HttpPost]
        //public IActionResult Create(CreateViewModel model)


        public async Task<IActionResult> Create(IFormFile files, CreateViewModel model)
        //=======================================================================
        //--- azurowe rozwiazanie ---
        {
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            byte[] dataFiles;
            // Pobiera konto magazynu z parametrów połączenia =  UseDevelopmentStorage=true;

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);


            // --- tworzenie blob client
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            // --- pobiera odwołanie do kontenera
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filescontainers");


            // --- tworzy kontener jezeli nie istnieje

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

            string systemFileName = files.FileName;
            await using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                dataFiles = target.ToArray();
                // files.FileName
            }
            // To również nie powoduje wezwania serwisu; tworzy tylko obiekt lokalny

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
            await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

            //systemFileName.
            string nazwa_bloaba1; //dorobilem1
            nazwa_bloaba1 = cloudBlockBlob.Uri.ToString();
            string nazwa_bloaba; //dorobilem1

            nazwa_bloaba = files.FileName.ToString();  // dorobilem1
            //nazwa_bloaba = files.Name.ToString(); // dorobilem1
            //return View();

            //--- z HomeControler ---
            //=========================================================================
            //{
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model); // to przerobic na link uri blob.Uri


                Book newBook = new Book
                {
                    Title = model.Title,
                    Date = model.Date,
                    ItemType = model.ItemType,
                    // do cover podac link uri blob.Uri
                    //  Cover = uniqueFileName,
                    Cover = nazwa_bloaba1, // dodalem1 to jest z bloba nazwa
                    Status = model.Status
                };

                _bookRepository.Add(newBook);
                return RedirectToAction("Details", new { id = newBook.Id });
            }

            return View();
        }
        //------------------- koniec HomeControler Create ----
        //==========================================================
        //...........................................................

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
        // przerobione na aync Task do blobow 
        public async Task<IActionResult> Edit(IFormFile files, BookEditViewModel model)
        {
            //--- Azur ---
            //--- azurowe rozwiazanie ---
            {
                string blobstorageconnection = _configuration.GetValue<string>
                ("blobstorage");
                byte[] dataFiles;
                // Pobiera konto magazynu z parametrów połączenia =
                //UseDevelopmentStorage = true;

                CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(blobstorageconnection);

                // --- tworzenie blob client
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                // --- pobiera odwołanie do kontenera
                CloudBlobContainer cloudBlobContainer =
                cloudBlobClient.GetContainerReference("filescontainers");

                // --- tworzy kontener jezeli nie istnieje
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
                string systemFileName = files.FileName;
                await using (var target = new MemoryStream())
                {
                    files.CopyTo(target);
                    dataFiles = target.ToArray();
                    // files.FileName
                }
                // To również nie powoduje wezwania serwisu; tworzy tylko
                //obiekt lokalny
                CloudBlockBlob cloudBlockBlob =
                cloudBlobContainer.GetBlockBlobReference(systemFileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0,
                dataFiles.Length);
                //systemFileName.
                string nazwa_bloaba1; //dorobilem1
                nazwa_bloaba1 = cloudBlockBlob.Uri.ToString();
                string nazwa_bloaba; //dorobilem1
                nazwa_bloaba = files.FileName.ToString(); // dorobilem1

                //---  z HomeControler
                if (ModelState.IsValid)
                {
                    Book book = _bookRepository.Get(model.Id);
                    book.Title = model.Title;
                    book.Date = model.Date;
                    book.ItemType = model.ItemType;
                    book.AuthorName = model.AuthorName;
                    book.Status = model.Status;
                    // dodalem cover
                    book.Cover = nazwa_bloaba1; // dorobilem1 pobiera adres uri bloba
                    /*
                    if (model.Photo != null)
                    {
                        if (model.ExistingPhotoPath != null)
                       {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                               "files", model.ExistingPhotoPath);
                            System.IO.File.Delete(filePath);
                        }
                        book.Cover = ProcessUploadedFile(model);
                    }
                    */
                    _bookRepository.Update(book);
                    return RedirectToAction("index");
                }
                return View();
            }


        }

        //=========================================================
        //--------------- wyswietlanie listy blobow ----------
        // link https://localhost:44367/Demo/ShowAllBlobs

        public async Task<IActionResult> ShowAllBlobs()
        {
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            CloudStorageAccount cloudStorageAccount =
            CloudStorageAccount.Parse(blobstorageconnection);
            // Create the blob client.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container =
            blobClient.GetContainerReference("filescontainers");
            CloudBlobDirectory dirb = container.GetDirectoryReference("filescontainers");
            BlobResultSegment resultSegment = await
            container.ListBlobsSegmentedAsync(string.Empty,
            true, BlobListingDetails.Metadata, 100, null, null, null);
            List<FileData> fileList = new List<FileData>();
            foreach (var blobItem in resultSegment.Results)
            {
                // A flat listing operation returns only blobs, not virtual directories.
                var blob = (CloudBlob)blobItem;
                fileList.Add(new FileData()
                {
                    //FileName = blob.StorageUri.ToString(), // podaje Primary i Secondary link 
                    // Primary = 'http://127.0.0.1:10000/devstoreaccount1/filescontainers/100_6186.JPG'; 
                    // Secondary = 'http://127.0.0.1:10000/devstoreaccount1-secondary/filescontainers/100_6186.JPG'
                    FileName = blob.Name, // podaje nazwe 100_6186.JPG

                    //  FileName = blob.Uri.ToString(),  // podaje Primary link:
                    // http://127.0.0.1:10000/devstoreaccount1/filescontainers/100_6186.JPG
                    // to ma wrzucac do bazy danych Cover jest to  gotowy link do wyswietlania strony :) 
                    //FileName = blob.Container

                    FileSize = Math.Round((blob.Properties.Length / 1024f) / 1024f, 2).ToString(),
                    ModifiedOn = DateTime.Parse(blob.Properties.LastModified.ToString()).ToLocalTime().ToString()
                });
            }
            return View(fileList);
        }
        // dorobie wysweitlania :
        // taki link z bloba wyswietla w wioku:
        //  <img src="http://127.0.0.1:10000/devstoreaccount1/filescontainers/100_6186.JPG" />
        // clel przerowbic tak zeby wyeitlalo zdkcjia z blobow
        // z tego FileName = blob.Uri.ToString(),  wrzucac do bazy danych jest to w kontrolerze
        // BlobController w Task<IActionResult> ShowAllBlobs()  
        // jak juz wrzuci do bazy sql z tamtad bedzie pobieralo link do bloba

        //---------------download pobieranie  ------------

        public async Task<IActionResult> Download(string blobName)
        {
            CloudBlockBlob blockBlob;
            await using (MemoryStream memoryStream = new MemoryStream())
            {
                string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("filescontainers");
                blockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
                await blockBlob.DownloadToStreamAsync(memoryStream);
            }

            Stream blobStream = blockBlob.OpenReadAsync().Result;
            return File(blobStream, blockBlob.Properties.ContentType, blockBlob.Name);
        }
        //---------------   delete kasowanie-------------
        // <a href="/Demo/Download?blobName=@data.FileName">Download</a>

        public async Task<IActionResult> Delete(string blobName)
        {
            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "filescontainers";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            var blob = cloudBlobContainer.GetBlobReference(blobName);
            await blob.DeleteIfExistsAsync();
            return RedirectToAction("ShowAllBlobs", "Home"); // z home pokazuje dane
        }

        // ----------------- kopia z  HomeControler Create ---

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

        // -- dodalem z homeControler 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // koniec dodane z chomecontroler --    
    }

}

