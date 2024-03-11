using CodingTest.Core.Interfaces.Integration_Services;
using CodingTest.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodingTest.Web.Controllers
{
    using CodingTest.Core.Models;

    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;
        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }
        [HttpGet("{stationId}")]
        public async Task<IActionResult> GetRainfallData(string stationId)
        {
            if (!long.TryParse(stationId, out long numericStationId))
            {
                var errorResponse = new ErrorResponse
                                        {
                                            Message = "Station ID must be a number.",
                                            Detail = new List<List<ErrorDetail>>
                                                         {
                                                             new List<ErrorDetail> { new ErrorDetail { PropertyName = "stationId", Message = "Invalid format. Station ID must be a numeric value." } }
                                                         }
                                        };
                return BadRequest(errorResponse);
            }
            var result = await _rainfallService.GetProcessedRainfallDataAsync(stationId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            // Log the error or handle it as needed
            if (result.Error.Message.Contains("not found"))
            {
                return NotFound(result.Error);
            }
            else
            {
                return StatusCode(500, result.Error);
            }
        }
    }
}