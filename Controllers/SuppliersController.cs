using EventMangerServerApi.Application.Interfaces;
using EventMangerServerApi.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventMangerServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound($"Supplier with ID {id} not found.");
            }

            return Ok(supplier);
        }

        [HttpPost]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<IActionResult> AddSupplier([FromBody] CreateSupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _supplierService.AddSupplierAsync(supplierDto);
                return CreatedAtAction(nameof(GetAllSuppliers), new { id = supplierDto.Name }, supplierDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _supplierService.UpdateSupplierAsync(id, dto);
                if (!success)
                    return NotFound($"Supplier with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "MustBeEventManager")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var success = await _supplierService.DeleteSupplierAsync(id);
                if (!success)
                {
                    return NotFound($"Supplier with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
