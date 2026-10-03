using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Vasileva_crosspl.Services;

namespace Vasileva_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly Service _service;

        public ReportsController(Service service)
        {
            _service = service;
        }

        [HttpGet("urgent-repairs")]
        public IActionResult GetUrgentRepairs()
        {
            var result = _service.GetUrgentRepairs();
            return Ok(result);
        }

        [HttpGet("contractor-costs")]
        public IActionResult GetContractorCosts()
        {
            var result = _service.GetContractorCosts();
            return Ok(result);
        }

        [HttpGet("all-records-details")]
        public IActionResult GetAllRecordsDetails()
        {
            var result = _service.GetAllRecordsDetails();
            return Ok(result);
        }
    }
}