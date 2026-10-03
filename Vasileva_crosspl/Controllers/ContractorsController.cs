using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Vasileva_crosspl.Models;
using Vasileva_crosspl.Services;

namespace Vasileva_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorsController : ControllerBase
    {
        private readonly Service _service;

        public ContractorsController(Service service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contractor>>> GetContractors()
        {
            var contractors = await _service.GetAllContractors();
            return Ok(contractors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contractor>> GetContractor(int id)
        {
            var contractor = await _service.GetContractorById(id);
            if (contractor == null) return NotFound();
            return Ok(contractor);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Contractor>> PostContractor(Contractor contractor)
        {
            var created = await _service.CreateContractor(contractor);
            return CreatedAtAction(nameof(GetContractor), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutContractor(int id, Contractor contractor)
        {
            var success = await _service.UpdateContractor(id, contractor);
            if (!success) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteContractor(int id)
        {
            var success = await _service.DeleteContractor(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}