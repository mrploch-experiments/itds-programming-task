using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Itds.CardActionsMicroservice.Business.Model;
using Moq;

namespace Itds.CardActionsMicroservice.WebApi.IntegrationTests.Endpoints;

public class CardActionsEndpointsV1Tests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public CardActionsEndpointsV1Tests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.CardServiceMock.Setup(s => s.GetCardDetailsAsync("UserA", "Card_1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CardDetails("Card_1", CardType.Prepaid, CardStatus.Active, true));
        _factory.CardServiceMock.Setup(s => s.GetCardDetailsAsync("UserA", "Card_2", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CardDetails("Card_2", CardType.Debit, CardStatus.Ordered, true));
        _factory.CardServiceMock.Setup(s => s.GetCardDetailsAsync("UserA", "Card_3", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CardDetails("Card_3", CardType.Credit, CardStatus.Inactive, true));
        _factory.CardServiceMock.Setup(s => s.GetCardDetailsAsync("UserA", "Card_4", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CardDetails("Card_4", CardType.Credit, CardStatus.Ordered, true));
        _factory.CardServiceMock.Setup(s => s.GetCardDetailsAsync("UserB", "Card_5", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CardDetails("Card_5", CardType.Credit, CardStatus.Ordered, false));
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetCardActions_should_return_actions_for_existing_user_and_existing_card()
    {
        var cardActions = await _httpClient.GetFromJsonAsync<CardActions>("/v1/cardactions?userId=UserA&cardNumber=Card_1");
        var cardActions2 = await _httpClient.GetFromJsonAsync<CardActions>("/v1/cardactions?userId=UserB&cardNumber=Card_5");

        cardActions.Should().NotBeNull();
        cardActions!.UserId.Should().Be("UserA");
        cardActions.CardNumber.Should().Be("Card_1");
        cardActions.AllowedActions.ToHashSet()
                   .Should()
                   .BeEquivalentTo("ACTION1", "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13");

        cardActions2.Should().NotBeNull();
        cardActions2!.UserId.Should().Be("UserB");
        cardActions2.CardNumber.Should().Be("Card_5");
        cardActions2.AllowedActions.Should().BeEquivalentTo("ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13");
    }

    [Fact]
    public async Task GetCardActions_should_return_not_found_for_non_existing_user()
    {
        var httpResponseMessage = await _httpClient.GetAsync("/v1/cardactions?userId=UserX&cardNumber=Card23");
        httpResponseMessage.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCardActions_should_return_bad_request_when_no_parameters_are_supplied()
    {
        var httpResponseMessage = await _httpClient.GetAsync("/v1/cardactions");
        httpResponseMessage.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }
}