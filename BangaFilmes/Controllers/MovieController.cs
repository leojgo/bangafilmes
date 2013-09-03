using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BangaFilmes.Models;
using BangaFilmes.ViewModels;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

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
                    Assigned=false
                });
            }
            ViewBag.Genres = viewModel;
            return View();
        }

        //
        // POST: /Movie/Create

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        //
        // GET: /Movie/Edit/5

        public ActionResult Edit(int id = 0)
        {
            /*Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }*/

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
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Movie/Search

       
        public ActionResult Search()
        {            
            return View();
        }

        //
        // POST: /Movie/Search/name

        [HttpPost]
        public ActionResult Search(string name)
        {
            var tmdbApi = new TMDbClient("470fd2ec8853e25d2f8d86f685d2270e", false, "http://api.themoviedb.org/3/");

            tmdbApi.GetConfig();
            var results = tmdbApi.SearchMovie("Matrix","pt");
            //SearchContainer<SearchMovie> results = tmdbApi.SearchMovie("Matrix");
            return View(results.Results);
            //SearchContainer<SearchMovie> results = tmdbApi.SearchMovie(name);

            //return View(results.Results);
        }
    }
}