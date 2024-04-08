using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApiTest.Controllers;
using Xunit;

[CollectionDefinition("CustomWebApplicationFactory collection")]
public class CustomWebApplicationFactoryCollection 
   : ICollectionFixture<CustomWebApplicationFactory<Microsoft.VisualStudio.TestPlatform.TestHost.Program>>
{
   // This class has no code, and is never created. Its purpose is simply
   // to be the place to apply [CollectionDefinition] and all the
   // ICollectionFixture<> interfaces.
}