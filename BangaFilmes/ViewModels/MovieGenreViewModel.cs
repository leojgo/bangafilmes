using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BangaFilmes.ViewModels
{
    public class MovieGenreViewModel
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}