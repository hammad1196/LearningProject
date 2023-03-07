using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44354/api/");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Display()
        {
            var response = await _httpClient.GetAsync("employee");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonContent = await response.Content.ReadAsStringAsync();

            var employees = JsonSerializer.Deserialize<List<Employee>>(jsonContent, options);

            return View(employees);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Employee employee)
        {

            var json = JsonSerializer.Serialize(employee);

            var content = new StringContent(json,Encoding.UTF8,"application/json");


            var response = await _httpClient.PostAsync("employee/addEmployee",content);


            if(!response.IsSuccessStatusCode)
            {
                return NotFound(response);
            }
            else
            {
                return RedirectToAction("Display");

            }

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await _httpClient.DeleteAsync($"employee/DeleteEmployee?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                 NotFound(response);
            }
            return RedirectToAction("Display");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"employee/GetEmployeeById?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonContent = await response.Content.ReadAsStringAsync();
            var current_employee = JsonSerializer.Deserialize<Employee>(jsonContent, options);

            return View(current_employee);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Employee employee)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("employee/EditEmployee", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound(response);
            }
            else
            {
                return RedirectToAction("Display");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> GetDeptEmployees(string depName)
        {

            var response = await _httpClient.GetAsync($"employee/GetDeptEmployees?department_name={depName}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonContent = await response.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<List<Employee>>(jsonContent, options);
            return View(employees);
        }
    }
}
