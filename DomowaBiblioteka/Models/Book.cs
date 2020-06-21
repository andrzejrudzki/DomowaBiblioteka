using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.Models
{
    public class Book
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public ItemType ItemType { get; set; }    
        public string Cover { get; set; }
        public string AuthorName { get; set; }
    }

}

