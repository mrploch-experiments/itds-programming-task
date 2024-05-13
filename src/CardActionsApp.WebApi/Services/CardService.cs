using CardActionsApp.WebApi.Model;

namespace CardActionsApp.WebApi.Services;

public class CardService : ICardService
{
    private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards;
    public CardService()
    {
        _userCards = GenerateSampleData();
    }

    public async Task<Dictionary<string, Dictionary<string, CardDetails>>> GetAllCardDetails()
    {
        return _userCards;
    }
    
    public async Task<CardDetails?> GetCardDetails(string userId, string cardNumber)
    {
// At this point, we would typically make an HTTP call to an external service
// to fetch the data. For this example we use generated sample data.
        await Task.Delay(1000);
        if (_userCards.TryGetValue(userId, out var cards)
            && cards.TryGetValue(cardNumber, out var cardDetails))
        {
            return cardDetails;
        }
        return null;
    }
    private static Dictionary<string, Dictionary<string, CardDetails>> GenerateSampleData()
    {
        var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();
        for (var i = 1; i <= 3; i++)
        {
            var cards = new Dictionary<string, CardDetails>();
            var cardIndex = 1;
            foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            {
                foreach (CardStatus cardStatus in Enum.GetValues(typeof(CardStatus)))
                {
                    cards.Add($"Card{cardIndex}",
                              new CardDetails(CardNumber: $"Card{cardIndex}",
                                              CardType: cardType,
                                              CardStatus: cardStatus,
                                              IsPinSet: cardIndex % 2 == 0));
                    cardIndex++;
                }
            }
            userCards.Add($"User{i}", cards);
        }
        return userCards;
    }
}