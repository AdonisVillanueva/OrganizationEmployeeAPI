using Microsoft.AspNetCore.Mvc;
using OrganizationEmployeeAPI.Models;
using OrganizationEmployeeAPI.Contracts;
using System;

namespace OrganizationEmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployee _employee;
        private readonly IDepartment _department;

        public EmployeeController(IWebHostEnvironment env,
            IEmployee employee,
            IDepartment department)
        {
            _env = env;
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _department = department ?? throw new ArgumentNullException(nameof(department));
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _employee.GetEmployees());
            }            
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeByID")]
        public async Task<IActionResult> GetEmployeeByID(int id)
        {
            try
            {
                return Ok(await _employee.GetEmployeeByID(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> Post(Employee emp)
        {
            try
            {
                var result = await _employee.InsertEmployee(emp);
                return Ok("Added Successfully");
            }  
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }


        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> Put(Employee emp)
        {
            try
            {
                var result = await _employee.UpdateEmployee(emp);
                return Ok("Updated Successfully");
            }
             catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            _employee.DeleteEmployee(id);
            return new JsonResult("Deleted Successfully");
        }


        [HttpPost]
        [Route("SaveFile")]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;


                if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                {
                    _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var uploads = Path.Combine(_env.WebRootPath, "Photos");
                var filePath = Path.Combine(uploads, postedFile.FileName);

                if (!Directory.Exists(uploads))
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }


        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartmentNames()
        {
            return Ok(await _department.GetDepartments());
        }
    }
}
