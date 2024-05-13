using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using FluentAssertions;

namespace CardActionsApp.Business.Tests.Services;

public class CardActionsServiceTests
{
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { CardType.Prepaid, CardStatus.Ordered, true, new[] { 3, 4, 6, 8, 9, 10, 12, 13 } },
            new object[] { CardType.Prepaid, CardStatus.Ordered, false, new[] { 3, 4, 7, 8, 9, 10, 12, 13 } },
            new object[] { CardType.Prepaid, CardStatus.Blocked, true, new[] { 3, 4, 6, 7, 8, 9 } },
            new object[] { CardType.Prepaid, CardStatus.Inactive, true, new[] { 2, 3, 4, 6, 8, 9, 10, 11, 12, 13 } },
            new object[] { CardType.Prepaid, CardStatus.Inactive, false, new[] { 2, 3, 4, 7, 8, 9, 10, 11, 12, 13 } },
            new object[] { CardType.Credit, CardStatus.Blocked, true, new[] { 3, 4, 5, 6, 7, 8, 9 } },
            new object[] { CardType.Credit, CardStatus.Blocked, false, new[] { 3, 4, 5, 8, 9 } },
            new object[] { CardType.Prepaid, CardStatus.Closed, true, new[] { 3, 4, 9 } },
            new object[] { CardType.Prepaid, CardStatus.Closed, false, new[] { 3, 4, 9 } }
        };

    [Theory]
    [MemberData(nameof(Data))]
    public async Task GetAllowedActions_should_returned_allowed_actions_for_particual_card_data1(CardType cardType,
                                                                                                 CardStatus cardStatus,
                                                                                                 bool isPinSet,
                                                                                                 params int[] expectedActionNumbers)
    {
        var sut = new CardActionsService();

        (await sut.GetAllowedActions(new CardDetails("", cardType, cardStatus, isPinSet))).Should()
                                                                                          .BeEquivalentTo(expectedActionNumbers.Select(a => $"ACTION{a}").ToList());
    }
}