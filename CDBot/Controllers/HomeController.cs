using Microsoft.AspNetCore.Mvc;

namespace Controller;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{

    public string Index()
    {
        return "Server is running!";
    }
}