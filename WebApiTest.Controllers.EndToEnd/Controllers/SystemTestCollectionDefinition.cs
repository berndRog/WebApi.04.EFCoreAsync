using Xunit;
namespace WebApiTest.Controllers;

[CollectionDefinition(nameof(SystemTestCollectionDefinition), DisableParallelization = true)]
public class SystemTestCollectionDefinition { }