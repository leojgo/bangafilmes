using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BangaFilmes.Models
{
    public class MovieCatalogInitializer : DropCreateDatabaseIfModelChanges<MovieCatalog>
    {
        protected override void Seed(MovieCatalog context)
        {
            var filmes = new List<Movie>
            {
                new Movie { OriginalTitle="Argo",PosterUrl=@"/capas/Argo.jpg" },
                new Movie { OriginalTitle="Avatar",PosterUrl=@"/capas/Avtar.jpg" },
                new Movie { OriginalTitle="Batman",PosterUrl=@"/capas/Batman.jpg" },
                new Movie { OriginalTitle="Devil",PosterUrl=@"/capas/Devil.jpg" },
                new Movie { OriginalTitle="Extracted",PosterUrl=@"/capas/Extracted.jpg" }                
            };

            filmes.ForEach(f => context.Movies.Add(f));
            context.SaveChanges();

            var generos = new List<Genre>
            {
                new Genre { Id=12, Name="Aventura" },
                new Genre { Id=14, Name="Fantasia" },
                new Genre { Id=16, Name="Animação" },
                new Genre { Id=18, Name="Drama" },
                new Genre { Id=22, Name="Musical" },
                new Genre { Id=27, Name="Terror" },
                new Genre {Id=28,Name="Ação" },
                new Genre {Id=35,Name="Comédia" }
            };

            generos.ForEach(g => context.Genres.Add(g));
            context.SaveChanges();
        }
    }

}