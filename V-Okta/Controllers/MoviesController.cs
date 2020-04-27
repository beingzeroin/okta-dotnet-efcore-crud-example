using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace V_Okta.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        Data.VoktaContext _context;

        public MoviesController(Data.VoktaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new Models.MoviesHomeModel(_context.Movies.Include("Votes").ToList(), getUserId());
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
                    model.Movie = new Models.Movie(movie, getUserId());
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveMovie(Models.Movie movie)
        {
            if (movie.Id > 0)
            {
                var data = _context.Movies.Where(r => r.Id.Equals(movie.Id)).FirstOrDefault();

                if (data == null)
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
            try
            {
                var movie = _context.Movies.Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (movie == null)
                    throw new Exception("no movie found");

                _context.Movies.Remove(movie);
                _context.SaveChanges();

                return new JsonResult(true);
            }
            catch (Exception e)
            {
                return new JsonResult(false);
            }
        }

        [HttpPost]
        public IActionResult UpvoteMovie(int id)
        {
            try
            {
                var movie = _context.Movies.Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (movie == null)
                    throw new Exception("no movie found");

                var vote = _context.Votes.Where(r => r.UserId.Equals(getUserId()) && r.MovieId.Equals(id)).FirstOrDefault();

                if (vote == null)
                {
                    _context.Votes.Add(new Data.Entities.Vote()
                    {
                        MovieId = id,
                        UserId = getUserId(),
                        Value = 1
                    });

                    movie.CurrentVotes += 1;
                }
                else
                {
                    movie.CurrentVotes -= vote.Value;

                    if (vote.Value == 1)
                    {
                        vote.Value = 0;
                    }
                    else
                    {
                        vote.Value = 1;
                        movie.CurrentVotes += 1;
                    }
                }

                _context.SaveChanges();

                return new JsonResult(new Models.VoteMovieResponse()
                {
                    CurrentVotes = movie.CurrentVotes,
                    UserVote = vote.Value,
                    Success = true
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new Models.VoteMovieResponse()
                {
                    Success = false
                });
            }
        }

        [HttpPost]
        public IActionResult DownvoteMovie(int id)
        {
            try
            {
                var movie = _context.Movies.Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (movie == null)
                    throw new Exception("no movie found");

                var vote = _context.Votes.Where(r => r.UserId.Equals(getUserId()) && r.MovieId.Equals(id)).FirstOrDefault();

                if (vote == null)
                {
                    _context.Votes.Add(new Data.Entities.Vote()
                    {
                        MovieId = id,
                        UserId = getUserId(),
                        Value = -11
                    });

                    movie.CurrentVotes -= 1;
                }
                else
                {
                    movie.CurrentVotes -= vote.Value;

                    if (vote.Value == -1)
                    {
                        vote.Value = 0;
                    }
                    else
                    {
                        vote.Value = -1;
                        movie.CurrentVotes -= 1;
                    }                    
                }

                _context.SaveChanges();

                return new JsonResult(new Models.VoteMovieResponse()
                {
                    CurrentVotes = movie.CurrentVotes,
                    UserVote = vote.Value,
                    Success = true
                });
            }
            catch (Exception e)
            {
                return new JsonResult(new Models.VoteMovieResponse()
                {
                    Success = false
                });
            }
        }

        protected int getUserId()
        {
            var username = (User.Identity as ClaimsIdentity).Claims.Where(r => r.Type.Equals("preferred_username")).First().Value;
            return _context.Users.Where(r => r.Username.Equals(username)).First().Id;
        }
    }
}