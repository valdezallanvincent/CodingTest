using CodingTest.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Core.External_API_Clients
{
    // IRainfallClientService.cs
    public interface IRainfallClientService
    {
        Task<ServiceResult<RainfallApiResponse>> GetRainfallDataAsync(string stationId);
    }


}
