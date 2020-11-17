using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using static DomowaBiblioteka.Common.Enums.Enums;

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
