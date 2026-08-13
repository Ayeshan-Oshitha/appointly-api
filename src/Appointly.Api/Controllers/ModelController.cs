using Appointly.Application.DTOs.Model;
using Appointly.Application.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }


        [HttpGet]
        public async Task<IActionResult> GetModels([FromQuery] Guid? brandId)
        {
            var models = await _modelService.GetModels(brandId);
            return Ok(models);
        }


        [HttpPost]
        public async Task<IActionResult> CreateModel([FromBody] CreateModelRequestDto request)
        {
            var model = await _modelService.AddModel(request);
            return Ok(model);
        }


        [HttpPut("{modelId}")]
        public async Task<IActionResult> UpdateModel([FromRoute] Guid modelId, [FromBody] UpdateModelRequestDto request)
        {
            var model = await _modelService.UpdateModel(modelId, request);
            return Ok(model);
        }


        [HttpDelete("{modelId}")]
        public async Task<IActionResult> DeleteModel([FromRoute] Guid modelId)
        {
            await _modelService.DeleteModel(modelId);
            return Ok("Model deleted successfully");
        }

    }
}
