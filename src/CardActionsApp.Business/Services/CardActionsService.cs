using CardActionsApp.WebApi.Model;

namespace CardActionsApp.WebApi.Services;

public record CardActionDecision(string Name, HashSet<CardType> CardTypes, HashSet<CardStatus> CardStatuses, bool? PinSet);

public class CardActionsService : ICardActionsService
{
    private readonly IList<CardActionDecision> _cardActionDecisions = BuildDecisionTable();

    private static CardActionDecisionCardTypesBuilder Add(string actionName)
    {
        return new CardActionDecisionCardTypesBuilder(actionName);
    }

    private static IList<CardActionDecision> BuildDecisionTable()
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

    public async Task<IEnumerable<string>> GetAllowedActions(CardDetails cardDetails)
    {
        ArgumentNullException.ThrowIfNull(cardDetails, nameof(cardDetails));
        
        var allowedActions = new HashSet<string>();

        foreach (var cardActionDecision in _cardActionDecisions)
        {
            // var status = cardDetails.CardStatus;
            // if (cardDetails.IsPinSet.HasValue)
            // {
            //     status = cardDetails.IsPinSet.Value ? cardDetails.CardStatus | CardStatus.PinSet : cardDetails.CardStatus | CardStatus.PinUnset;
            // }
            
            var status = cardDetails.IsPinSet ? cardDetails.CardStatus | CardStatus.PinSet : cardDetails.CardStatus | CardStatus.PinUnset;
            // if (cardDetails.IsPinSet.HasValue)
            // {
            //     status = cardDetails.IsPinSet.Value ? cardDetails.CardStatus | CardStatus.PinSet : cardDetails.CardStatus | CardStatus.PinUnset;
            // }

            if (cardActionDecision.CardTypes.Contains(cardDetails.CardType) &&
                (cardActionDecision.CardStatuses.Contains(cardDetails.CardStatus) || cardActionDecision.CardStatuses.Contains(status)))
            {
                allowedActions.Add(cardActionDecision.Name);
            }
        }

        return allowedActions;
    }
}

public class CardActionDecisionsBuilder
{ }

public class CardActionDecisionCardTypesBuilder
{
    private readonly string _actionName;

    public CardActionDecisionCardTypesBuilder(string actionName)
    {
        _actionName = actionName;
    }

    public CardActionDecisionCardStatusesBuilder ForCardTypes(params CardType[] cardTypes)
    {
        return new CardActionDecisionCardStatusesBuilder(_actionName, cardTypes);
    }
}

public class CardActionDecisionCardStatusesBuilder
{
    private readonly string _actionName;
    private readonly IEnumerable<CardType> _cardTypes;

    public CardActionDecisionCardStatusesBuilder(string actionName, IEnumerable<CardType> cardTypes)
    {
        _actionName = actionName;
        _cardTypes = cardTypes;
    }

    public CardActionDecisionPinSetBuilder ForCardStatuses(params CardStatus[] cardStatuses)
    {
        return new CardActionDecisionPinSetBuilder(_actionName, _cardTypes, cardStatuses);
    }
}

public class CardActionDecisionPinSetBuilder
{
    private readonly string _actionName;
    private readonly IEnumerable<CardStatus> _cardStatuses;
    private readonly IEnumerable<CardType> _cardTypes;

    public CardActionDecisionPinSetBuilder(string actionName, IEnumerable<CardType> cardTypes, IEnumerable<CardStatus> cardStatuses)
    {
        _actionName = actionName;
        _cardTypes = cardTypes;
        _cardStatuses = cardStatuses;
    }

    public CardActionDecision PinSet(bool? pinSet)
    {
        return new CardActionDecision(_actionName, new HashSet<CardType>(_cardTypes), new HashSet<CardStatus>(_cardStatuses), pinSet);
    }
}