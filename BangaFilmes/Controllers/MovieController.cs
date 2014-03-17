using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BangaFilmes.Models;
using BangaFilmes.ViewModels;
using System.Data.Entity.Infrastructure;

namespace BangaFilmes.Controllers
{
    public class MovieController : BootstrapBaseController
    {
        private MovieCatalog db = new MovieCatalog();

        //
        // GET: /Movie/

        public ActionResult Index()
        {
            return View(db.Movies.ToList());
        }

        //
        // GET: /Movie/Details/5

        public ActionResult Details(int id = 0)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        //
        // GET: /Movie/Create

        public ActionResult Create()
        {
            var allGenres = db.Genres;
            var viewModel = new List<MovieGenreViewModel>();
            foreach (var genre in allGenres)
            {
                viewModel.Add(new MovieGenreViewModel
                {
                    GenreId = genre.Id,
                    Name = genre.Name,
                    Assigned = false
                });
            }
            ViewBag.Genres = viewModel;
            return View();
        }

        //
        // POST: /Movie/Create

        [HttpPost, ActionName("Create")]
        public ActionResult Create(Movie movie, string[] selectedGenres)
        {
            if (ModelState.IsValid)
            {
                movie.PosterUrl = "http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w92" + movie.PosterUrl;
                UpdateMovieGenres(selectedGenres, movie);
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        //
        // GET: /Movie/Edit/5

        public ActionResult Edit(int? id = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies
                            .Include(i => i.Genres).Single(i => i.Id == id);
            PopulateAssignedGenreData(movie);
            return View(movie);
        }

        private void PopulateAssignedGenreData(Movie movie)
        {
            var allGenres = db.Genres;
            var movieGenres = new HashSet<int>(movie.Genres.Select(c => c.Id));
            var viewModel = new List<MovieGenreViewModel>();
            foreach (var genre in allGenres)
            {
                viewModel.Add(new MovieGenreViewModel
                {
                    GenreId = genre.Id,
                    Name = genre.Name,
                    Assigned = movieGenres.Contains(genre.Id)
                });
            }
            ViewBag.Genres = viewModel;
        }

        //
        // POST: /Movie/Edit/5

        [HttpPost]
        public ActionResult Edit(int? id, string[] selectedGenres)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var movieToUpdate = db.Movies
                .Include(m => m.Genres)
                .Where(m => m.Id == id)
                .Single();

            if (TryUpdateModel(movieToUpdate))
            {
                try
                {

                    db.Entry(movieToUpdate).State = EntityState.Modified;
                    UpdateMovieGenres(selectedGenres, movieToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (EntityException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedGenreData(movieToUpdate);
            return View(movieToUpdate);


        }

        private void UpdateMovieGenres(string[] selectedGenres, Movie movieToUpdate)
        {
            if (selectedGenres == null)
            {
                movieToUpdate.Genres = new List<Genre>();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var movieGenres = new HashSet<int>
                (movieToUpdate.Genres.Select(c => c.Id));
            foreach (var genre in db.Genres)
            {
                if (selectedGenresHS.Contains(genre.Id.ToString()))
                {
                    if (!movieGenres.Contains(genre.Id))
                    {
                        movieToUpdate.Genres.Add(genre);
                    }
                }
                else
                {
                    if (movieGenres.Contains(genre.Id))
                    {
                        movieToUpdate.Genres.Remove(genre);
                    }
                }
            }
        }

        //
        // GET: /Movie/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        //
        // POST: /Movie/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }*/

        //
        // GET: /Movie/Search


        public ActionResult Search()
        {
            return View();
        }

        //
        // POST: /Movie/Search/name

        //[HttpPost]
        //public ActionResult Search(string name)
        //{
        //    var tmdbApi = new TMDbClient("470fd2ec8853e25d2f8d86f685d2270e", false, "http://api.themoviedb.org/3/");

        //    tmdbApi.GetConfig();
        //    var results = tmdbApi.SearchMovie("Matrix","pt");
        //    //SearchContainer<SearchMovie> results = tmdbApi.SearchMovie("Matrix");
        //    return View(results.Results);
        //    //SearchContainer<SearchMovie> results = tmdbApi.SearchMovie(name);

        //    //return View(results.Results);
        // }
    }
}