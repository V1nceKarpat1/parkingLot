using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using parkingLotAPI.DTOs;
using parkingLotAPI.Services;
using parkingLotAPI.Utils;

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
                return NotFound(response.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostReservation(CreateReservation createData)
        {
            var response = await reservationService.NewReservation(createData);

            if (response.ResponseStatus == ResponseStatus.BAD_REQUEST)
            {
                return BadRequest(response.Message);
            }
            else
            {
                return NoContent();
            }
        }
    }


}
