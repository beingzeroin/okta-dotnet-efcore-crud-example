using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace V_Okta.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        Data.VoktaContext _context;
        int _userId = 0;

        public MoviesController(Data.VoktaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new Models.MoviesHomeModel(_context.Movies.ToList(), _userId);
            return View(model);
        }

        [Route("/Movies/Manage/{id?}")]
        public IActionResult Manage(int id = 0)
        {
            var model = new Models.ManageMovieModel();
            model.RequestedId = id;

            if (id.Equals(0))
                model.NewMovie = true;

            else
            {
                var movie = _context.Movies.Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (movie != null)
                {
                    model.Movie = new Models.Movie(movie, _userId);
                }
            }           

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveMovie(Models.Movie movie)
        {
            if(movie.Id > 0)
            {
                var data = _context.Movies.Where(r => r.Id.Equals(movie.Id)).FirstOrDefault();

                if(data == null)
                {
                    throw new Exception("movie not found");
                }

                data.Description = movie.Description;
            }
            else
            {
                _context.Movies.Add(new Data.Entities.Movie()
                {
                    Description = movie.Description,
                    Title = movie.Title
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies", null);
        }

        [HttpPost]
        public IActionResult RemoveMovie(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult UpvoteMovie(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult DownvoteMovie(int id)
        {
            throw new NotImplementedException();
        }
    }
}