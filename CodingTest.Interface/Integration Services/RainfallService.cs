using AutoMapper;
using CodingTest.Infrastructure.External_API_Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Infrastructure.Integration_Services
{
    using CodingTest.Core.External_API_Clients;
    using CodingTest.Core.Interfaces.Integration_Services;
    using CodingTest.Core.Models;

    public class RainfallService : IRainfallService
    {
        private readonly IRainfallClientService _clientService;
        private readonly IMapper _mapper;
        private readonly ILogger<RainfallService> _logger;
        public RainfallService(IRainfallClientService clientService, IMapper mapper, ILogger<RainfallService> logger)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ServiceResult<RainfallDto>> GetProcessedRainfallDataAsync(string stationId)
        {
            var result = await _clientService.GetRainfallDataAsync(stationId);
            if (result.IsSuccess)
            {
                var dto = _mapper.Map<RainfallDto>(result.Data);
                return ServiceResult<RainfallDto>.Success(dto);
            }
            else
            {
                return ServiceResult<RainfallDto>.Failure(result.Error);
            }
        }
    }
}
