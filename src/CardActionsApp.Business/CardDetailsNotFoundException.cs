namespace CardActionsApp.Business;

public class CardDetailsNotFoundException : Exception
{
    public string UserId { get; }

    public string CardId { get; }

    public CardDetailsNotFoundException(string userId, string cardId, string message, Exception? innerException = null) : base(message, innerException)
    {
        UserId = userId;
        CardId = cardId;
    }
}