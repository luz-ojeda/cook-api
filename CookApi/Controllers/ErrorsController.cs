using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace cook_api.Controllers;
[ApiController]
public class ErrorsController : ControllerBase
{
    ILogger<ErrorsController> logger;
    public ErrorsController(ILogger<ErrorsController> logger)
    {
        this.logger = logger;
    }

    [Route("/error")]
    public ActionResult Error([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>();
        logger.LogError(exceptionHandlerFeature?.Error.ToString());

        return Problem(
            detail: "Please try again later or contact the owner of the repository if the problem persists",
            title: "We have troubles serving your request at the moment");

    }
    [Route("/error-development")]
    public ActionResult DevelopmentError([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>();
        logger.LogError(exceptionHandlerFeature?.Error.ToString());

        return Problem(
            detail: exceptionHandlerFeature?.Error.StackTrace,
            title: exceptionHandlerFeature?.Error.Message);

    }
}