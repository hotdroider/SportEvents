using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportEvents.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEvents.API.Sports
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        public SportsController(SportEventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        SportEventsDbContext _dbContext;

        /// <summary>
        /// Список всех видов спорта с подсчетом количества событий по ним
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<SportsDTO>>> Get()
        {
            // subquerry instead of group by...
            return await _dbContext.Sports.Select(s => new SportsDTO()
            {
                Name = s.Name,
                SportId = s.SportId,
                EventsCount = s.Events.Count()
            }).ToListAsync();
        }
    }
}