using System;
using System.ComponentModel.DataAnnotations;
using static DomowaBiblioteka.Common.Enums.Enums;

namespace DomowaBiblioteka.Models
{
    //ToDo: change this model to viewmodel
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
    }
}

