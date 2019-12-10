using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SongsProject.Controllers
{
    public class ErrorController : Controller
    {
        //Inject instance of ILogger - where you need the logging functionality
        private readonly ILogger<ErrorController> _logger;

        // Inject ASP.NET Core ILogger service. Specify the Controller
        // Type as the generic parameter. This helps us identify later
        // which class or controller has logged the exception
        //Since we have specified the type of ErrorController as the generic argument for ILogger, 
        // the fully qualified name of ErrorController is also included in the log output below.
        // We will see all errors place on output window.
        public ErrorController(ILogger<ErrorController> logger)
        {
            //The type of the class or controller into which ILogger is injected can be specified as the argument for the generic parameter of ILogger. 
            //We do this because, the fully qualified name of the class or the controller is then included in the log output as the log category. 
            //Log category is used to group the log messages.
            _logger = logger;
        }

        //Handling error 500 - Handling error server did not know how to handle
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            // Retrieve the exception Details(Path and Error)
            var exceptionHandlerPathFeature =
                  HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            // LogError() method logs the exception under Error category in the log
            _logger.LogError($"The path {exceptionHandlerPathFeature.Path} " +
                $"threw an exception {exceptionHandlerPathFeature.Error}");

            return View("Error");
        }

        // If there is 404 status code, the route path will become Error/404
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            //StatusCodeResult will show us path problem and query string problem
            var statusCodeResult =
           HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    // LogWarning() method logs the message under
                    // Warning category in the log
                    _logger.LogWarning($"404 error occured. Path = " +
                        $"{statusCodeResult.OriginalPath} and QueryString = " +
                        $"{statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("NotFound");
        }

    }
}
