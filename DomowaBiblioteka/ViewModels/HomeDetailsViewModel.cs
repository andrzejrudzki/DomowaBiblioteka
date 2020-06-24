using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomowaBiblioteka.Models.Enums;

namespace DomowaBiblioteka.ViewModels
{
    public class HomeDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public ItemType ItemType { get; set; }
        public string Cover { get; set; }
        public string AuthorName { get; set; }
    }
}
