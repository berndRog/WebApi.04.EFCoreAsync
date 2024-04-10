using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
namespace WebApiTest.Controllers;
public static class THelper {
   
   // public static OkObjectResult ResultFromResponseOwner<OkObjectResult>(
   //    ActionResult<IEnumerable<OwnerDto>> response
   // ) where OkObjectResult : class{
   //    response.Should().NotBeNull();
   //    response.Result.Should().BeOfType<OkObjectResult>();
   //    var result = response.Result as OkObjectResult;
   //    result.Should().NotBeNull();
   //    return result!;
   // }
   
   private static (bool, T, S) EvalActionResult<T, S>(
      ActionResult<S?> actionResult
   ) where T : ObjectResult   // OkObjectResult 
     where S : class? {       // OwnerDto
      
      // Check if actionResult is of type ObjectResult
      actionResult.Result.Should().NotBeNull().And.BeOfType<T>();
      // and cast it to ObjectResult
      var result = (actionResult.Result as T)!; 
      
      // Check if value is not null
      result.Value.Should().NotBeNull();
      // and result.Value is of Type s, then cast it to S
      if (result.Value is S resultValue) {
         // return true and result:T and resultValue:S
         return (true, result, resultValue); 
      }
      // return false and result:T and default(S)
      return (false, result, default!);
   }
   
   // HttpStatusCode.Ok (200)
   public static void IsOk<T>(
      ActionResult<T?> actionResult, 
      T? expected
   ) where T : class? {
      var(success, result, value) =  
         EvalActionResult<OkObjectResult, T?>(actionResult);
      success.Should().BeTrue();
      result.StatusCode.Should().Be(200);
      value.Should().NotBeNull().And.BeEquivalentTo(expected);
   }
   
   // HttpStatusCode.Created (201)
   public static void IsCreated<T>(
      ActionResult<T?> actionResult, 
      T? expected
   )  where T : class? {
      var(success, result, value) = 
         EvalActionResult<CreatedResult, T?>(actionResult);
      success.Should().BeTrue();
      result.StatusCode.Should().Be(201);
      value.Should().NotBeNull().And.BeEquivalentTo(expected);
   }
   
   // HttpStatusCode.NoContent (204)
   public static void IsNoContent(
      IActionResult actionResult
   ) {
      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<NoContentResult>();
   }
   
   // HttpStatusCode.NotFound (404)
   public static void IsNotFound<T>(
      ActionResult<T?> actionResult
   ) where T : class? {
      var(success, result, value) =  
         EvalActionResult<NotFoundObjectResult, T?>(actionResult);
      success.Should().BeFalse();
      result.StatusCode.Should().Be(404);
   }
   
   // HttpStatusCode.Conflict (409)
   public static void IsConflict<T>(
      ActionResult<T?> actionResult
   ) where T : class? {
      var(success,result,value) = 
         EvalActionResult<ConflictObjectResult, T?>(actionResult);
      success.Should().BeFalse();
      result.StatusCode.Should().Be(409);
   }
}
