using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace UnitTestDll;

public class IntegrationTests
{
    public IntegrationTests()
    {
    }

    [Fact]
    public async Task LogonTest()
    {
        var host = new WebHostBuilder();
    }
}