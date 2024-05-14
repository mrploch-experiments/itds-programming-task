namespace Itds.CardActionsMicroservice.Business;

public class CardDetailsNotFoundException(string userId, string cardId, string message, Exception? innerException = null) : Exception(message, innerException)
{
    public string UserId { get; } = userId;

    public string CardId { get; } = cardId;
}