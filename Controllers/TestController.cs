using Microsoft.AspNetCore.Mvc;
namespace TodoProj.Controllers;


[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{

    [HttpGet]
    public string Get()
    {
        return "Hello World";
    }
}