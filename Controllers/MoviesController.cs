using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace CinemaApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MoviesController : ControllerBase
  {
    private CinemaDbContext _dbContext;
    public MoviesController(CinemaDbContext dbContext) {
      _dbContext = dbContext;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Post([FromForm] Movie movieObj)
    {
      var guid = Guid.NewGuid();
      var filePath = Path.Combine("wwwroot", guid + ".jpg");
      if (movieObj.Image != null)
      {
        var fileStream = new FileStream(filePath, FileMode.Create);
        movieObj.Image.CopyTo(fileStream);
      }
      movieObj.ImageUrl = filePath.Remove(0, 7);
      _dbContext.Movies.Add(movieObj);
      _dbContext.SaveChanges();

      return StatusCode(StatusCodes.Status201Created);
    }

    // PUT: api/<MoviesController>/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromForm] Movie movieObj)
    {
      var movie = _dbContext.Movies.Find(id);
      if (movie == null)
      {
        return NotFound("No record found against this id");
      }
      else
      {
        var guid = Guid.NewGuid();
        var filePath = Path.Combine("wwwroot", guid + ".jpg");
        if (movieObj.Image != null)
        {
          var fileStream = new FileStream(filePath, FileMode.Create);
          movieObj.Image.CopyTo(fileStream);
          movieObj.ImageUrl = filePath.Remove(0, 7);
        }

        movie.Name = movieObj.Name;
        movie.Description = movieObj.Description;
        movie.Language = movieObj.Language;
        movie.Duration = movieObj.Duration;
        movie.PlayingDate = movieObj.PlayingDate;
        movie.PlayingTime = movieObj.PlayingTime;
        movie.Rating = movieObj.Rating;
        movie.Genre = movieObj.Genre;
        movie.TrailorUrl = movieObj.TrailorUrl;
        movie.TicketPrice = movieObj.TicketPrice;
        _dbContext.SaveChanges();
        return Ok("Record updated successfuly");
      }
    }

  }
}