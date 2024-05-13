using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using FluentAssertions;

namespace CardActionsApp.WebApi.Tests.Services;

public class CardActionsServiceTests
{
    [Fact]
    public async Task GetAllowedActions_should_return_allowed_card_actions_for_card_details()
    {
        var sut = new CardActionsService();
        var allowedActions = await sut.GetAllowedActions(new CardDetails(Guid.NewGuid().ToString(), CardType.Credit, CardStatus.Ordered, false));
        
        allowedActions.Should().HaveCount(1).And.Contain("ACTION3");
    }
}