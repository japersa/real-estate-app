using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Dto;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _service;

        public OwnerController(IOwnerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _service.GetPagedAsync(pageNumber, pageSize);

            var response = new
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / pageSize)
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var owner = await _service.GetByIdAsync(id);
            return owner == null ? NotFound() : Ok(owner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OwnerDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Owner created");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] OwnerDto dto)
        {
            if (dto.Id != null && id != dto.Id)
                return BadRequest("Id does not match");

            try
            {
                await _service.UpdateAsync(id, dto);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
