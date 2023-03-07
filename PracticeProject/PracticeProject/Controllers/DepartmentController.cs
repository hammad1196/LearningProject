using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticeProject.Models;

namespace PracticeProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonDocument jsonContent;

        public DepartmentController(ILogger<DepartmentController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44354/api/");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Display()
        {
            var response = await _httpClient.GetAsync("Department");
            if(response == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var json = await response.Content.ReadAsStringAsync();
            var departments = JsonSerializer.Deserialize<List<Department>>(json, options);
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Department department)
        {
            var json = JsonSerializer.Serialize(department);
            var content = new StringContent(json, Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync("Department/AddDepartment", content);
            if(!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            return RedirectToAction("Display");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var response = _httpClient.DeleteAsync($"Department/DeleteDepartment?id={id}");
            if(response == null)
            {
                return NotFound(response);
            }
            return RedirectToAction("Display");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Department/DepartmentId?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var json = await response.Content.ReadAsStringAsync();
            var department = JsonSerializer.Deserialize<Department>(json, options);

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Department department)
        {
            var json = new StringContent(JsonSerializer.Serialize(department), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Department/EditDepartment", json);
            if (response == null)
            {
                return NotFound();
            }
            return RedirectToAction("Display");
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentsName()
        {
            var response = await _httpClient.GetAsync("Department/GetDepartmentsName");
            if (response == null)
            {
                return NotFound();
            }
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            List<string> names = JsonSerializer.Deserialize<List<string>>(json, options);
            var departments = names.Select(name => new Department { Department_Name = name }).ToList();
            //var length = departments.Count;
            //for (int i = 0; i < length; i++)
            //{
            //    departments[0].Id = ;
            //}
            return View(departments);
        }
    }
}
