using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System.Threading.Tasks;
using App.Logics;
using Domain.Controllers;
using Domain.OutfaceModels;
using App.Services;
using Domain.Exceptions;

namespace StaffManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : BaseController
    {
        private readonly StaffLogic _staffLogic;

        public StaffController(StaffLogic staffLogic)
        {
            _staffLogic = staffLogic;
        }

        /// <summary>
        /// Create a new staff member
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _staffLogic.CreateAsync(model);
                return CreatedAtAction(nameof(GetStaffById), new { staffId = result.StaffId }, result);
            }
            catch (LogicException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get staff by ID
        /// </summary>
        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaffById(string staffId)
        {
            try
            {
                var result = await _staffLogic.GetByIdAsync(staffId);
                if (result == null)
                {
                    return NotFound(new { message = $"Staff with ID '{staffId}' not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Update existing staff
        /// </summary>
        [HttpPut("{staffId}")]
        public async Task<IActionResult> UpdateStaff(string staffId, [FromBody] UpdateStaffRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _staffLogic.UpdateAsync(staffId, model);
                return Ok(result);
            }
            catch (LogicException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete staff by ID
        /// </summary>
        [HttpDelete("{staffId}")]
        public async Task<IActionResult> DeleteStaff(string staffId)
        {
            try
            {
                var result = await _staffLogic.DeleteAsync(staffId);
                if (!result)
                {
                    return NotFound(new { message = $"Staff with ID '{staffId}' not found." });
                }

                return Ok(new { message = "Staff deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all staff members
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var result = await _staffLogic.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Advanced search for staff
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchStaff([FromQuery] StaffSearchRequestModel searchModel)
        {
            try
            {
                var result = await _staffLogic.SearchAsync(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Export all staff to Excel
        /// </summary>
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] StaffSearchRequestModel? searchModel = null)
        {
            try
            {
                var staffList = searchModel != null && HasSearchParameters(searchModel)
                    ? await _staffLogic.SearchAsync(searchModel)
                    : await _staffLogic.GetAllAsync();

                var excelData = await ExportService.ExportToExcelAsync(staffList);
                var fileName = $"staff_list_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Export all staff to PDF
        /// </summary>
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportToPdf([FromQuery] StaffSearchRequestModel? searchModel = null)
        {
            try
            {
                var staffList = searchModel != null && HasSearchParameters(searchModel)
                    ? await _staffLogic.SearchAsync(searchModel)
                    : await _staffLogic.GetAllAsync();

                var pdfData = await ExportService.ExportToPdfAsync(staffList);
                var fileName = $"staff_list_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return File(pdfData, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        private bool HasSearchParameters(StaffSearchRequestModel searchModel)
        {
            return !string.IsNullOrEmpty(searchModel.StaffId) ||
                   !string.IsNullOrEmpty(searchModel.FullName) ||
                   searchModel.Gender.HasValue ||
                   searchModel.BirthdayFrom.HasValue ||
                   searchModel.BirthdayTo.HasValue ||
                   searchModel.MinAge.HasValue ||
                   searchModel.MaxAge.HasValue;
        }
    }
}