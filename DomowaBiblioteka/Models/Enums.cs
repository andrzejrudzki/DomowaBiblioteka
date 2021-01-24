using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Models
{
    public class Enums
    {
        public enum ItemType

        {
            //DVD
            DVD,
            //CD
            CD,
            //Book
            Book
        }

        public enum StatusType

        {
            //On The Shelf
            Dostepna,
            //Loaned 
            Wypozyczony
        }

        public enum ItemTypeSearch

        {
            //select
            Wybierz,
            //DVD
            DVD,
            //CD
            CD,
            //Book
            Book
        }
    }
}
