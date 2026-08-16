using MotorHub.Application.DTOs.Model;
using MotorHub.Application.Services.Models;
using MotorHub.Domain.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorHub.Api.Controllers
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
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> CreateModel([FromBody] CreateModelRequestDto request)
        {
            var model = await _modelService.AddModel(request);
            return StatusCode(StatusCodes.Status201Created, model);
        }


        [HttpPut("{modelId:guid}")]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> UpdateModel([FromRoute] Guid modelId, [FromBody] UpdateModelRequestDto request)
        {
            var model = await _modelService.UpdateModel(modelId, request);
            return Ok(model);
        }


        [HttpDelete("{modelId:guid}")]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> DeleteModel([FromRoute] Guid modelId)
        {
            await _modelService.DeleteModel(modelId);
            return NoContent();
        }

    }
}
