using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
namespace WebApi.Controllers;

[ApiController]
public class ErrorsController : ControllerBase {

   #region methods
   [Route("/banking/error-development")]
// [MapToApiVersion("1.0")]
   [HttpGet]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesDefaultResponseType]
   public IActionResult HandleErrorDevelopment(
      [FromServices] IHostEnvironment hostEnvironment  
   ) {
      if (!hostEnvironment.IsDevelopment()) {
         return NotFound();
      }

      var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
      
      var values = exceptionHandlerFeature?.RouteValues?.Values;
      var text = string.Empty;
      if(values != null) {
         foreach(var r in values)
            if(r != null) text += r + " ";
      }
      text += $"{exceptionHandlerFeature?.Path} ... ";
      text += $"{exceptionHandlerFeature?.Error.Message}";

      return Problem(
         title: text,
         instance: exceptionHandlerFeature?.Endpoint?.DisplayName,
         detail: exceptionHandlerFeature?.Error?.StackTrace
      );
   }

   // RFC 7807 Probelm Details
   [Route("/banking/error")]
// [MapToApiVersion("1.0")]
   [HttpGet]
   [ProducesDefaultResponseType]
   public IActionResult HandleError()
      => Problem();
   #endregion
}
