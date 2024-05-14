using Itds.CardActionsMicroservice.Business.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Itds.CardActionsMicroservice.WebApi.IntegrationTests;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public Mock<ICardService> CardServiceMock { get; } = new();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
                                  {
                                      services.AddSingleton(CardServiceMock.Object);
                                  });

        return base.CreateHost(builder);
    }
}