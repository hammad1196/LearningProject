using LearningWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LearningWebApi.Controllers
{
    [Route("api/Department")]
    public class DepartmentController : Controller
    {
        private readonly LearningWebContext dc;

        public DepartmentController(LearningWebContext dc)
        {
            this.dc = dc;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await dc.Departments.ToListAsync();
            return Ok(departments);
        }

        [HttpGet("DepartmentId")]
        public async Task<IActionResult> GetDepartment(int? id)
        {
            var department = await dc.Departments.FindAsync(id);
            return Ok(department);
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            var new_department = new Department
            {
                Department_Name = department.Department_Name,
            };
            if (new_department == null)
            {
                return NotFound();
            }
            dc.Departments.Add(new_department);
            dc.SaveChanges();

            int id = new_department.Id;
            return await GetDepartment(id);
        }

        [HttpDelete("DeleteDepartment")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = dc.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            dc.Departments.Remove(department);
            dc.SaveChanges();
            return Ok();
        }

        [HttpPut("EditDepartment")]
        public IActionResult Edit([FromBody] Department department)
        {
            Department current_department = dc.Departments.FirstOrDefault(m => m.Id == department.Id);
            if (current_department == null)
            {
                return NotFound();
            }
            current_department.Department_Name = department.Department_Name;
            dc.Departments.Update(current_department);
            dc.SaveChanges();
            return Ok();
        }
        
        [HttpGet("GetDepartmentsName")]
        public IActionResult GetDepartmentsName()
        {
            var departments = dc.Departments.Select(d => d.Department_Name).Distinct().ToList();
            return Ok(departments);
        }
    }
}
