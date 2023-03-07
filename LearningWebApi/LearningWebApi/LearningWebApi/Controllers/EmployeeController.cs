using LearningWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningWebApi.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly LearningWebContext dc;
s        public EmployeeController(LearningWebContext dc)
        {
            this.dc = dc;
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await dc.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpGet("getEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee =  dc.Employees.FirstOrDefault(a => a.Id == id);
            return Ok(employee);

        }

        [HttpPost("addEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            var entity = new Employee
            {
                Name = model.Name,
                Designation = model.Designation,
                Department = model.Department,
                Salary = model.Salary,

            };

            dc.Employees.Add(entity);
            dc.SaveChanges();

            int id = entity.Id;

            return await GetEmployeeById(id);
        }

        [HttpDelete("DeleteEmployee")]
        public IActionResult DeleteEmployee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = dc.Employees.FirstOrDefault(m => m.Id == id);
            dc.Employees.Remove(employee);
            dc.SaveChanges();
            return Ok();
        }

        [HttpPut("EditEmployee")]
        public IActionResult Edit([FromBody] Employee employee)
        {
            Employee current_employee = dc.Employees.FirstOrDefault(m => m.Id == employee.Id);

            if (current_employee != null)
            {
                current_employee.Name = employee.Name;
                current_employee.Department = employee.Department;
                current_employee.Designation = employee.Designation;
                current_employee.Salary = employee.Salary;
                dc.Employees.Update(current_employee);
                dc.SaveChanges();
                return Ok(employee);

            }
            return NotFound();
        }

        [HttpGet("GetDeptEmployees")]
        public IActionResult GetEmployeesByDept(string department_name)
        {
            var employees = dc.Employees.Where(d => d.Department == department_name).ToList();
            return Ok(employees);
        }
    }
}
