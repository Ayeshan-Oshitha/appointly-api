using Appointly.Api.Common.DTOs.Advertisment;
using Appointly.Application.Services.Advertisements;
using Appointly.Application.Services.Advertisment.Contracts;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("advertisment")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertisementService;
        public AdvertisementController(IMapper mapper, IAdvertisementService advertisementService)
        {
            _mapper = mapper;
            _advertisementService = advertisementService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAdvertisements()
        {
            var advertisments = await _advertisementService.GetAllAdvertisments();
            return Ok(_mapper.Map<List<AdvertisementDetailResponseDto>>(advertisments));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvertisement([FromBody] CreateAdvertisementRequestDto request)
        {
            var advertisement = await _advertisementService.AddAdvertisment(_mapper.Map<CreateAdvertisementRequest>(request));
            return Ok(_mapper.Map<AdvertisementResponseDto>(advertisement));
        }
    }
}
