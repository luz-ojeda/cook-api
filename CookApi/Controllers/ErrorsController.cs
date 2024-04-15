using System.Data;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace cook_api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    private readonly ILogger<ErrorsController> logger;
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

        if (exceptionHandlerFeature?.Error is DuplicateNameException duplicateNameException)
        {
            return HandleDuplicateNameException(exceptionHandlerFeature);
        }

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

        if (exceptionHandlerFeature?.Error is DuplicateNameException duplicateNameException)
        {
            return HandleDuplicateNameException(exceptionHandlerFeature);
        }

        return Problem(
            detail: exceptionHandlerFeature?.Error.InnerException?.Message,
            title: exceptionHandlerFeature?.Error.Message
            );
    }

    private ObjectResult HandleDuplicateNameException(IExceptionHandlerFeature? exception)
    {

        return Problem(
            detail: exception?.Error.Message,
            statusCode: (int)HttpStatusCode.Conflict
            );

    }
}