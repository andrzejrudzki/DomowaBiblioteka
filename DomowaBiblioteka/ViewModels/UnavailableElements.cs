using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.ViewModels
{
    public class UnavailableElement
    {
        public DateTime DateOfRent { get; set; }
        public string RentalApprovingPerson { get; set; }
        public string Title { get; set; }
    }
}
