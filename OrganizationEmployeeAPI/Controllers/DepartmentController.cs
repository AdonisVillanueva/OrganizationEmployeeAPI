using Microsoft.AspNetCore.Mvc;
using OrganizationEmployeeAPI.Models;
using OrganizationEmployeeAPI.Contracts;

namespace OrganizationEmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartment _department;
        public DepartmentController(IDepartment department)
        {
            _department = department ?? throw new ArgumentNullException(nameof(department));
        }

        [HttpGet]
        [Route("GetDepartment")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _department.GetDepartments());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDepartmentByID/{Id}")]
        public async Task<IActionResult> GetDeptById(int Id)
        {
            try
            {
                return Ok(await _department.GetDepartmentByID(Id));
            }            
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddDepartment")]
        public async Task<IActionResult> Post(Department dep)
        {
            try
            {
                var result = await _department.InsertDepartment(dep);
                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }            
        }

        [HttpPut]
        [Route("UpdateDepartment")]
        public async Task<IActionResult> Put(Department dep)
        {
            try
            {
                await _department.UpdateDepartment(dep);
                return Ok("Updated Successfully");
            }            
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete]
        //[HttpDelete("{id}")]
        [Route("DeleteDepartment")]
        public JsonResult Delete(int id)
        {
            try
            {
                _department.DeleteDepartment(id);
                return new JsonResult("Deleted Successfully");
            }
            catch (Exception)
            {
                return new JsonResult("Deleted Failed");
            }
        }
    }
}
