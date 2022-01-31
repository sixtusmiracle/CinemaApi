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
  public class ReservationsController : ControllerBase
  {
    private CinemaDbContext _dbContext;
    public ReservationsController(CinemaDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    // api/reservations
    [HttpPost]
    [Authorize]
    public IActionResult Post([FromBody] Reservation reservationObj)
    {
      reservationObj.ReservationTime = DateTime.Now;
      _dbContext.Reservations.Add(reservationObj);
      _dbContext.SaveChanges();
      return StatusCode(StatusCodes.Status201Created);
    }
    
    // api/reservations
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetReservations() 
    {
      var reservations =  from reservation in _dbContext.Reservations
                          join customer in _dbContext.Users on reservation.UserId equals customer.Id
                          join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                          select new 
                          {
                            Id = reservation.Id,
                            ReservationTime = reservation.ReservationTime,
                            CustomerName = customer.Name,
                            MovieName = movie.Name
                          };

      return Ok(reservations);
    }

  }
}