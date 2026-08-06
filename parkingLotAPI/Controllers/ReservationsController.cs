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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await reservationService.GetAllSpotsAsync());
        }

        [HttpGet("{spotId}")]
        public async Task<IActionResult> GetById(string spotId)
        {
            var response = await reservationService.GetSpotByIdAsync(spotId);

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
        public async Task<IActionResult> NewReservation(CreateReservationInfo createData)
        {
            var response = await reservationService.PostReservationAsync(createData);

            if (response.ResponseStatus == ResponseStatus.BAD_REQUEST)
            {
                return BadRequest(response.Message);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet("history/{spotId}")]
        public async Task<IActionResult> GetHistory(string spotId)
        {
            var response = await reservationService.GetSpotHistoryAsync(spotId);

            if (response.ResponseStatus == ResponseStatus.OK)
            {
                return Ok(response.ResponseData);
            }
            else
            {
                return NotFound(response.Message);
            }
        }

        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            var response = await reservationService.DeleteReservationAsync(reservationId);

            if (response.ResponseStatus == ResponseStatus.NO_CONTENT)
            {
                return NoContent();
            }
            else
            {
                return NotFound(response.Message);
            }
        }
    }


}
