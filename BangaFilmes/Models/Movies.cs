using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;

namespace BangaFilmes.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public int? Runtime { get; set; }
        
        public string MoviePath { get; set; }
        public string SubtitlePath { get; set; }
      

        public virtual ICollection<Genre> Genres { get; set; }

        public Movie()
        {
            this.Genres = new List<Genre>();
        }
        

    }


    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IList<Movie> Movies {get; set;}

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class MovieCatalog : DbContext
    {
        public MovieCatalog() : base("Test"){}

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Entity<Genre>().Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
    
    
}