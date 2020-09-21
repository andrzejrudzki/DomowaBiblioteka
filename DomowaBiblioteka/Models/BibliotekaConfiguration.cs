using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
{
    public class BibliotekaConfiguration
    {
        public string BibliotekaApiUrl { get; set; }
        public string StorageConnectionString { get; set; }
        public string StorageContainerName { get; set; }
    }
}
