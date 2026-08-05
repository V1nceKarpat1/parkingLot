using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using parkingLotAPI.Utils;
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



        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpotById(string id)
        {
            var response = await reservationService.GetSpotByIdAsync(id);

            if (response.ResponseStatus == ResponseStatus.OK)
            {
                return Ok(response.ResponseData);
            }
            else
            {
                return NotFound($"Entry with ID: {id} not found");
            }
        }
    }


}
