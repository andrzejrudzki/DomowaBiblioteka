using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.ViewModels
{
    public class RentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Komentarz(kto wypożyczył)")]
        public string Borrower { get; set; }
    }
}
