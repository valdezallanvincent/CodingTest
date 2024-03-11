namespace CodingTest.Core.Interfaces.Integration_Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CodingTest.Core.Models;

    // IRainfallService.cs
    public interface IRainfallService
    {
        Task<ServiceResult<RainfallDto>> GetProcessedRainfallDataAsync(string stationId);
    }
}
