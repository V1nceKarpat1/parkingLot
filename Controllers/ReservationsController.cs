using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using parkingLotAPI.Services;

namespace parkingLotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController(IReservationService reservationService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSpots()
        {
            return Ok(await reservationService.GetSpotsAsync());
        }

    }
}
