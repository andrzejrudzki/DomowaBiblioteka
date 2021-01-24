using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole Tytuł jest wymagane.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Pole Data jest wymagane.")]
        public DateTime Date { get; set; }

        public ItemType ItemType { get; set; }    
        public string Cover { get; set; }
        public string AuthorName { get; set; }
        public StatusType Status { get; set; }

        public DateTime DateOfRent { get; set; }
        public string RentalApprovingPerson  { get; set; }
        public string Borrower { get; set; }

    }
}

