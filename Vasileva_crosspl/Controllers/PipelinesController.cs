using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Vasileva_crosspl.Models;
using Microsoft.AspNetCore.Authorization;
using Vasileva_crosspl.Services;

namespace Vasileva_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelinesController : ControllerBase
    {
        private readonly Service _service;

        public PipelinesController(Service service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pipeline>>> GetPipelines()
        {
            var pipelines = await _service.GetAllPipelines();
            return Ok(pipelines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pipeline>> GetPipeline(int id)
        {
            var pipeline = await _service.GetPipelineById(id);
            if (pipeline == null) return NotFound();
            return Ok(pipeline);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Pipeline>> PostPipeline(Pipeline pipeline)
        {
            var created = await _service.CreatePipeline(pipeline);
            return CreatedAtAction(nameof(GetPipeline), new { id = created.Id }, created); 


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutPipeline(int id, Pipeline pipeline)
        {
            var success = await _service.UpdatePipeline(id, pipeline);
            if (!success) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePipeline(int id)
        {
            var success = await _service.DeletePipeline(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
