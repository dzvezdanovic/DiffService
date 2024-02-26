using DiffService.src.models;
using DiffService.src.services;
using Microsoft.AspNetCore.Mvc;

namespace DiffService.src.DiffService.Api
{
    [ApiController]
    [Route("v1/diff")]
    public class DiffController : ControllerBase
    {
        private readonly Dictionary<string, (byte[], byte[])> dataStore;
        private readonly IDiffService _service;

        public DiffController(IDiffService service)
        {
            _service = service;
            dataStore = _service.DataStore;
        }

        [HttpPost]
        [Route("{ID}/left")]
        public IActionResult Left(string ID, [FromBody] DiffData diffData)
        {
            var result = _service.Left(ID, diffData);
            if (result) return StatusCode(201);
            return BadRequest();
        }

        [HttpPost]
        [Route("{ID}/right")]
        public IActionResult Right(string ID, [FromBody] DiffData diffData)
        {
            var result = _service.Right(ID, diffData);
            if (result) return StatusCode(201);
            return BadRequest();
        }

        [HttpGet]
        [Route("{ID}")]
        public IActionResult Diff(string ID)
        {
            var result = _service.Diff(ID);

            return result;
        }
    }
}
