using Itds.CardActionsMicroservice.Business.Model;

namespace Itds.CardActionsMicroservice.Business.Services;

public record CardActionDecision(string Name, HashSet<CardType> CardTypes, HashSet<CardStatus> CardStatuses);

public class CardActionsService : ICardActionsService
{
    private readonly IList<CardActionDecision> _cardActionDecisions = BuildDecisionTable();

    public async Task<IEnumerable<string>> GetAllowedActionsAsync(CardDetails cardDetails, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(cardDetails, nameof(cardDetails));

        await Task.Delay(100, cancellationToken);

        var allowedActions = new HashSet<string>();

        foreach (var cardActionDecision in _cardActionDecisions)
        {
            var status = cardDetails.IsPinSet ? cardDetails.CardStatus | CardStatus.PinSet : cardDetails.CardStatus | CardStatus.PinUnset;

            if (cardActionDecision.CardTypes.Contains(cardDetails.CardType) &&
                (cardActionDecision.CardStatuses.Contains(cardDetails.CardStatus) || cardActionDecision.CardStatuses.Contains(status)))
            {
                allowedActions.Add(cardActionDecision.Name);
            }
        }

        return allowedActions;
    }

    private static CardActionDecisionCardTypesBuilder Add(string actionName)
    {
        return new CardActionDecisionCardTypesBuilder(actionName);
    }

    private static List<CardActionDecision> BuildDecisionTable()
    {
        return new List<CardActionDecision>
               {
                   Add("ACTION1").ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit).ForCardStatuses(CardStatus.Active).PinSet(null),
                   Add("ACTION2").ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit).ForCardStatuses(CardStatus.Inactive).PinSet(null),
                   Add("ACTION3")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered,
                                        CardStatus.Inactive,
                                        CardStatus.Active,
                                        CardStatus.Restricted,
                                        CardStatus.Blocked,
                                        CardStatus.Expired,
                                        CardStatus.Closed)
                       .PinSet(null),
                   Add("ACTION4")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered,
                                        CardStatus.Inactive,
                                        CardStatus.Active,
                                        CardStatus.Restricted,
                                        CardStatus.Blocked,
                                        CardStatus.Expired,
                                        CardStatus.Closed)
                       .PinSet(null),
                   Add("ACTION5")
                       .ForCardTypes(CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered,
                                        CardStatus.Inactive,
                                        CardStatus.Active,
                                        CardStatus.Restricted,
                                        CardStatus.Blocked,
                                        CardStatus.Expired,
                                        CardStatus.Closed)
                       .PinSet(null),
                   Add("ACTION6")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered | CardStatus.PinSet,
                                        CardStatus.Inactive | CardStatus.PinSet,
                                        CardStatus.Active | CardStatus.PinSet,
                                        CardStatus.Blocked | CardStatus.PinSet)
                       .PinSet(null),
                   Add("ACTION7")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered | CardStatus.PinUnset,
                                        CardStatus.Inactive | CardStatus.PinUnset,
                                        CardStatus.Active | CardStatus.PinUnset,
                                        CardStatus.Blocked | CardStatus.PinSet)
                       .PinSet(null),
                   Add("ACTION8")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked)
                       .PinSet(null),
                   Add("ACTION9")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered,
                                        CardStatus.Inactive,
                                        CardStatus.Active,
                                        CardStatus.Restricted,
                                        CardStatus.Blocked,
                                        CardStatus.Expired,
                                        CardStatus.Closed)
                       .PinSet(null),
                   Add("ACTION10")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active)
                       .PinSet(null),
                   Add("ACTION11").ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit).ForCardStatuses(CardStatus.Inactive, CardStatus.Active).PinSet(null),
                   Add("ACTION12")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active)
                       .PinSet(null),
                   Add("ACTION13")
                       .ForCardTypes(CardType.Prepaid, CardType.Debit, CardType.Credit)
                       .ForCardStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active)
                       .PinSet(null)
               };
    }

    private sealed class CardActionDecisionCardTypesBuilder(string actionName)
    {
        public CardActionDecisionCardStatusesBuilder ForCardTypes(params CardType[] cardTypes)
        {
            return new CardActionDecisionCardStatusesBuilder(actionName, cardTypes);
        }
    }

    private sealed class CardActionDecisionCardStatusesBuilder(string actionName, IEnumerable<CardType> cardTypes)
    {
        public CardActionDecisionPinSetBuilder ForCardStatuses(params CardStatus[] cardStatuses)
        {
            return new CardActionDecisionPinSetBuilder(actionName, cardTypes, cardStatuses);
        }
    }

    private sealed class CardActionDecisionPinSetBuilder(string actionName, IEnumerable<CardType> cardTypes, IEnumerable<CardStatus> cardStatuses)
    {
        public CardActionDecision PinSet(bool? pinSet)
        {
            return new CardActionDecision(actionName, [..cardTypes], [..cardStatuses]);
        }
    }
}