using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers;
public static class Helper {
   
   public static (bool, T, S) ResultFromResponse<T, S>(
      ActionResult<S> response
   ) where T : ObjectResult   // OkObjectResult 
     where S : class {        // OwnerDto
      
      response.Result.Should().NotBeNull().And.BeOfType<T>();
      T result = (response.Result as T)!; 
      
      result.Value.Should().NotBeNull();
      if (result.Value is S) {
         S value = (result.Value as S)!;
         return (true, result, value); 
      }
      else {
         return (false, result, default(S)!);
      }
   }
}
