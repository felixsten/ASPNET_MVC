using ASPNET_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ASPNET_MVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Home";




        return View();
    }

    
    public IActionResult Subscribe()
    {


        return RedirectToAction("Index", "Home");
    }

    
    [HttpPost]
    public async Task<IActionResult> Subscribe(SubscriberModel model)
    {
        if (ModelState.IsValid)
        {
            using var http = new HttpClient();

            var json = JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync($"https://localhost:7215/api/subscribers?email={model.Email}", content);


        }

        return RedirectToAction("Index", "Home");
    }
}
