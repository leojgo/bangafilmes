using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangaFilmes.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }        
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public int? Runtime { get; set; }        
        public double? VoteAverage { get; set; }
        public int? VoteCount { get; set; }

        public virtual IList<Genre> Genres { get; set; }

        public Movie()
        {
            this.Genres = new List<Genre>();
        }
    }


    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IList<Movie> Movies {get; private set;}
    }

    public class MovieCatalog : DbContext
    {
        //public MovieCatalog() : base("TesteConnection"){}

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}