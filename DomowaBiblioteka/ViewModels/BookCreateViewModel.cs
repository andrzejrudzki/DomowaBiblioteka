using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.ViewModels
{
    public class CreateViewModel
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Data")]
        public DateTime Date { get; set; }
        [Display(Name = "Nośnik")]
        public ItemType ItemType { get; set; }
        [Display(Name = "Autor/Reżyser")]
        public string AuthorName { get; set; }
        [Display(Name = "Zdjęcie")]
        public IFormFile Photo { get; set; }
        [Display(Name = "Status")]
        public StatusType Status { get; set; }
    }
}
