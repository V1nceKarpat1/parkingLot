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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
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
        [HttpGet("history/{id}")]
        public async Task<IActionResult> GetHistory(string id)
        {
            var response = await reservationService.GetSpotHistoryAsync(id);

            if (response.ResponseStatus == ResponseStatus.OK)
            {
                return Ok(response.ResponseData);
            }
            else
            {
                return NotFound(response.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var response = await reservationService.DeleteReservationAsync(id);

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
