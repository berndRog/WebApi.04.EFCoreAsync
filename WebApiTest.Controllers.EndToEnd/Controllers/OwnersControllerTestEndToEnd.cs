using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using WebApi.Core.Dto;
using Xunit;
namespace WebApiTest.Controllers;

[Collection("CustomWebApplicationFactory collection")]
public class OwnersControllerTestEndToEnd(
   CustomWebApplicationFactory<Microsoft.VisualStudio.TestPlatform.TestHost.Program> factory
) {

   private readonly HttpClient _client = factory.CreateClient();

   [Fact]
   public async Task GetOwners_ReturnsOkResult_WithAListOfOwnerDtos()
   {
      // Arrange
      var request = new HttpRequestMessage(HttpMethod.Get, "/banking/owners");

      // Act
      var response = await _client.SendAsync(request);

      // Assert
      response.StatusCode.Should().Be(HttpStatusCode.OK);

      var content = await response.Content.ReadAsStringAsync();
      var ownerDtos = JsonConvert.DeserializeObject<List<OwnerDto>>(content);
      ownerDtos.Should().NotBeEmpty();
   }
}