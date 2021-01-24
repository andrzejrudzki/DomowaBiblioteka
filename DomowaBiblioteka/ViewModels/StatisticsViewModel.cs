using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.ViewModels
{
    public class StatisticsViewModel
    {
        public int elementsNumber { get; set; }
        public int elementsNumberDVD { get; set; }
        public int elementsNumberCD { get; set; }
        public int elementsNumberBook { get; set; }
        public int elementsNumberAvailable { get; set; }
        public int elementsNumberUnavailable { get; set; }
        public List<UnavailableElement> unavailableElements { get; set; }
       }
}
