using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.ViewModels
{
    public class CreateViewModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public ItemType ItemType { get; set; }
        public string AuthorName { get; set; }
        public IFormFile Photo { get; set; }
    }
}
