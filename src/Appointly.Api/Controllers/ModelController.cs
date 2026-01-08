using Appointly.Api.Common.DTOs.Models;
using Appointly.Application.Services.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IModelService _modelService;

        public ModelController(IMapper mapper, IModelService modelService)
        {
            _mapper = mapper;
            _modelService = modelService;
        }


        [HttpGet]
        public async Task<IActionResult> GetModels([FromQuery] Guid? brandId)
        {
            var models = await _modelService.GetModels(brandId);
            return Ok(_mapper.Map<List<ModelResponseDto>>(models));
        }


        [HttpPost]
        public async Task<IActionResult> CreateModel([FromBody] CreateModelRequestDto request)
        {
            var model = await _modelService.AddModel(request.Name, request.BrandId);
            return Ok(_mapper.Map<ModelResponseDto>(model));
        }


        [HttpPut("{modelId}")]
        public async Task<IActionResult> UpdateModel([FromRoute] Guid modelId, [FromBody] UpdateModelRequestDto request)
        {
            var model = await _modelService.UpdateModel(modelId, request.Name, request.BrandId);
            return Ok(_mapper.Map<ModelResponseDto>(model));
        }


        [HttpDelete("{modelId}")]
        public async Task<IActionResult> DeleteModel([FromRoute] Guid modelId)
        {
            await _modelService.DeleteModel(modelId);
            return Ok("Model deleted successfully");
        }

    }   
        
        
}
