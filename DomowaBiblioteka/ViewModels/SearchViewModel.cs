using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Nośnik")]
        public ItemTypeSearch ItemType { get; set; }
        [Display(Name = "Autor/Reżyser")]
        public string AuthorName { get; set; }
    }
}
