using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportEvents.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEvents.API.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        public EventsController(SportEventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        SportEventsDbContext _dbContext;

        /// <summary>
        /// Информация по событию, получается по идентификатору
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<EventDTO>> Get(long eventID)
        {
            var ev = await _dbContext.Events.Include(e => e.Sport).FirstOrDefaultAsync(e => e.EventId == eventID);

            if (ev == null)
                return NotFound();

            return new EventDTO()
            {
                SportId = ev.SportId,
                SportName = ev.Sport.Name,
                EventId = ev.EventId,
                Name = ev.Name,
                EventDate = ev.EventDate,
                Status = ev.Status.ToString(),
                DrawPrice = ev.DrawPrice,
                Team1Price = ev.Team1Price,
                Team2Price = ev.Team2Price
            };
        }

        /// <summary>
        /// Список событий по спорту на дату
        /// </summary>
        /// <returns></returns>
        [HttpGet("{sportID}/{date}")]
        public async Task<ActionResult<List<EventDTO>>> GetBySportOnDate(int sportID, DateTime date)
        {
            return await _dbContext.Events.Include(e => e.Sport)
                .Where(ev => ev.SportId == sportID && ev.EventDate == date)
                .Select(ev => new EventDTO()
                {
                    SportId = ev.SportId,
                    SportName = ev.Sport.Name,
                    EventId = ev.EventId,
                    Name = ev.Name,
                    EventDate = ev.EventDate,
                    Status = ev.Status.ToString(),
                    DrawPrice = ev.DrawPrice,
                    Team1Price = ev.Team1Price,
                    Team2Price = ev.Team2Price
                }).ToListAsync();
        }
    }
}