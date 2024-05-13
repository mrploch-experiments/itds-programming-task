
using System.Net;
using System.Net.Http.Json;
using CardActionsApp.WebApi.Model;
using FluentAssertions;
using IntegrationTests.Helpers;

namespace CardActionsApp.WebApi.IntegrationTests;

public class CardActionsEndpointsV1Tests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;
    
    public CardActionsEndpointsV1Tests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetCardActions_should_return_actions_for_existing_user_and_existing_card()
    {
        var cardActions = await _httpClient.GetFromJsonAsync<CardActions>("/v1/cardactions?userId=User3&cardNumber=Card23");
        
        cardActions.Should().NotBeNull();
        cardActions.UserId.Should().Be("User3");
        cardActions.CardNumber.Should().Be("Card23");
        cardActions.AllowedActions.Should().NotBeEmpty().And.HaveCount(5).And.ContainEquivalentOf(new [] { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" });
    }
    
    [Fact]
    public async Task GetCardActions_should_return_not_found_for_non_existing_user()
    {
        var cardActions = await _httpClient.GetFromJsonAsync<CardActions>("/v1/cardactions?userId=UserX&cardNumber=Card23");

        var httpResponseMessage = await _httpClient.GetAsync("/v1/cardactions?userId=UserX&cardNumber=Card23");
        httpResponseMessage.Should().HaveStatusCode(HttpStatusCode.NotFound);
        // cardActions.Should().NotBeNull();
        // cardActions.UserId.Should().Be("User3");
        // cardActions.CardNumber.Should().Be("Card23");
        // cardActions.AllowedActions.Should().NotBeEmpty().And.HaveCount(5).And.ContainEquivalentOf(new [] { "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" });
    }
}