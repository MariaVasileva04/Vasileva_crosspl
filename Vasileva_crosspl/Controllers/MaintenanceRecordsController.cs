using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Vasileva_crosspl.Models;
using Vasileva_crosspl.Services;

namespace Vasileva_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRecordsController : ControllerBase
    {
        private readonly Service _service;

        public MaintenanceRecordsController(Service service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetRecords()
        {
            var records = await _service.GetAllRecords();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceRecord>> GetRecord(int id)
        {
            var record = await _service.GetRecordById(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<MaintenanceRecord>> PostRecord(MaintenanceRecord record)
        {
            var created = await _service.CreateRecord(record);
            return CreatedAtAction(nameof(GetRecord), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutRecord(int id, MaintenanceRecord record)
        {
            var success = await _service.UpdateRecord(id, record);
            if (!success) return BadRequest();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var success = await _service.DeleteRecord(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}