using System.Collections.Generic;
using System.Runtime.InteropServices;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ProductApp.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    public IProductService ProductService { get; }
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IProductService productService)
    {
        _logger = logger;
        ProductService = productService;
    }
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    //[HttpGet("BelongType")]
    //public async Task<List<BlongTypeDto>> GetBlongTypeDtosAsync()
    //{
    //   var dtos= await ProductService.GetBlongTypeDtosAsync();
    //   return dtos;
    //}
}
