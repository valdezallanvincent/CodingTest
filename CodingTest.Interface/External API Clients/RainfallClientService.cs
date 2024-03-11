using CodingTest.Core.External_API_Clients;
using CodingTest.Core.Models;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Infrastructure.External_API_Clients
{
    using System.Net.Http.Json;

    // RainfallClientService.cs
    public class RainfallClientService : IRainfallClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RainfallClientService> _logger;

        public RainfallClientService(HttpClient httpClient, ILogger<RainfallClientService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<RainfallApiResponse>> GetRainfallDataAsync(string stationId)
        {
            var policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                               .Or<HttpRequestException>()
                               .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                   onRetry: (outcome, timeSpan, retryCount, context) =>
                                   {
                                       if (outcome.Exception != null)
                                       {
                                           _logger.LogWarning(outcome.Exception, "Retrying [{RetryCount}] due to {ExceptionMessage}, waiting {TimeSpan}", retryCount, outcome.Exception.Message, timeSpan);
                                       }
                                       else
                                       {
                                           _logger.LogWarning("Retrying [{RetryCount}] due to HTTP {StatusCode}, waiting {TimeSpan}", retryCount, outcome.Result.StatusCode, timeSpan);
                                       }
                                   });

            try
            {
                var response = await policy.ExecuteAsync(async () => await _httpClient.GetAsync($"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}"));

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<RainfallApiResponse>();
                    return ServiceResult<RainfallApiResponse>.Success(apiResponse);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ServiceResult<RainfallApiResponse>.Failure(new ErrorResponse { Message = $"Station {stationId} not found.", Detail = new List<List<ErrorDetail>> { new List<ErrorDetail> { new ErrorDetail { Message = $"Station {stationId} not found.", PropertyName = "StationId"} } } });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<RainfallApiResponse>.Failure(new ErrorResponse { Message = $"Error fetching data: {errorContent}", Detail = new List<List<ErrorDetail>> { new List<ErrorDetail> { new ErrorDetail { Message = $"Error for fetching data for station {stationId}", PropertyName = "StationId" } } } });
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for station {StationId}", stationId);
                return ServiceResult<RainfallApiResponse>.Failure(new ErrorResponse { Message = $"Failed to fetch data from the external service for station {stationId}.", Detail = new List<List<ErrorDetail>> { new List<ErrorDetail> { new ErrorDetail { Message = ex.Message } } } });
            }
        }
    }
}
