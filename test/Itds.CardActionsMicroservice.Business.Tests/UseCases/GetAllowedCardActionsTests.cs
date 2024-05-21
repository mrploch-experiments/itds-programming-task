using AutoFixture.Xunit2;
using FluentAssertions;
using Itds.CardActionsMicroservice.Business.Model;
using Itds.CardActionsMicroservice.Business.Services;
using Itds.CardActionsMicroservice.Business.UseCases;
using Moq;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

namespace Itds.CardActionsMicroservice.Business.Tests.UseCases;

public class GetAllowedCardActionsTests
{
    [Theory]
    [AutoMockData]
    public async Task ExecuteAsync_should_retrieve_card_details_and_allowed_card_actions([Frozen] Mock<ICardService> cardService,
                                                                                         [Frozen] Mock<ICardActionsService> cardActionsService,
                                                                                         GetAllowedCardActions sut,
                                                                                         string userId,
                                                                                         string cardNumber,
                                                                                         CardType cardType,
                                                                                         CardStatus cardStatus,
                                                                                         bool isPinSet)
    {
        var cardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
        cardService.Setup(cs => cs.GetCardDetailsAsync(userId, cardNumber, CancellationToken.None)).ReturnsAsync(cardDetails);

        cardActionsService.Setup(cas => cas.GetAllowedActionsAsync(cardDetails, CancellationToken.None)).ReturnsAsync(new[] { "ACTION1", "ACTION3" });

        var result = await sut.ExecuteAsync(userId, cardNumber);

        result.CardNumber.Should().Be(cardNumber);
        result.UserId.Should().Be(userId);
        result.AllowedActions.Should().BeEquivalentTo(["ACTION1", "ACTION3"]);
    }

    [Theory]
    [AutoMockData]
    public async Task ExecuteAsync_should_retry_call_to_CardService_if_it_fails([Frozen] Mock<ICardService> cardService,
                                                                                [Frozen] Mock<ICardActionsService> cardActionsService,
                                                                                GetAllowedCardActions sut,
                                                                                string userId,
                                                                                string cardNumber,
                                                                                CardType cardType,
                                                                                CardStatus cardStatus,
                                                                                bool isPinSet)
    {
        var cardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);

        cardService.SetupSequence(cs => cs.GetCardDetailsAsync(userId, cardNumber, CancellationToken.None))
                   .ThrowsAsync(new Exception())
                   .ThrowsAsync(new Exception())
                   .ReturnsAsync(cardDetails);

        cardActionsService.Setup(cas => cas.GetAllowedActionsAsync(cardDetails, CancellationToken.None)).ReturnsAsync(new[] { "ACTION1", "ACTION3" });

        var result = await sut.ExecuteAsync(userId, cardNumber);

        result.CardNumber.Should().Be(cardNumber);
        result.UserId.Should().Be(userId);
        result.AllowedActions.Should().BeEquivalentTo(["ACTION1", "ACTION3"]);
    }

    [Theory]
    [AutoMockData]
    public async Task ExecuteAsync_should_retry_call_to_CardActionsService_if_it_fails([Frozen] Mock<ICardService> cardService,
                                                                                       [Frozen] Mock<ICardActionsService> cardActionsService,
                                                                                       GetAllowedCardActions sut,
                                                                                       string userId,
                                                                                       string cardNumber,
                                                                                       CardType cardType,
                                                                                       CardStatus cardStatus,
                                                                                       bool isPinSet)
    {
        var cardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);

        cardService.Setup(cs => cs.GetCardDetailsAsync(userId, cardNumber, CancellationToken.None)).ReturnsAsync(cardDetails);

        cardActionsService.SetupSequence(cas => cas.GetAllowedActionsAsync(cardDetails, CancellationToken.None))
                          .ThrowsAsync(new Exception())
                          .ThrowsAsync(new Exception())
                          .ReturnsAsync(new[] { "ACTION1", "ACTION3" });

        var result = await sut.ExecuteAsync(userId, cardNumber);

        result.CardNumber.Should().Be(cardNumber);
        result.UserId.Should().Be(userId);
        result.AllowedActions.Should().BeEquivalentTo(["ACTION1", "ACTION3"]);
    }
}