using ChinesePoker.Bots;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controller;

[ApiController]
[Route("")]
public class HomeController(IBot bot) : ControllerBase
{
    private const string ServerRunningMessage = "Server is running!";

    [HttpGet("health")]
    public string Index() => ServerRunningMessage;

    [HttpPost("move")]
    public PrivateModel? Move(MoveRequestModel request)
    {
        if (request.Private is null || request.Private.Input is null)
        {
            return null;
        }

        var result = bot.Arrange(request.Private.Input);
        GC.Collect();
        request.Private.Output = result;
        return request.Private;
    }
}