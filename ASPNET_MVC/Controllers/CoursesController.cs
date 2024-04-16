using ASPNET_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASPNET_MVC.Controllers;


public class CoursesController(HttpClient http) : Controller
{
    private readonly HttpClient _http = http;

    [Authorize]
    [Route("/courses")]
    [HttpGet]
    public async Task<IActionResult> CoursePage()
    {
        var viewModel = new CoursePageViewModel();


        var response = await _http.GetAsync("https://localhost:7215/api/courses");
        viewModel.Courses = JsonConvert.DeserializeObject<IEnumerable<CourseModel>>(await response.Content.ReadAsStringAsync())!;

        return View(viewModel);
    }
}
