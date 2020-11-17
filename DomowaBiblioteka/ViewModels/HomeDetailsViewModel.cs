﻿using System;
using static DomowaBiblioteka.Common.Enums.Enums;

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
        public StatusType Status { get; set; }
    }
}
