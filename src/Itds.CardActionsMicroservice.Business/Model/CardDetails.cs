namespace Itds.CardActionsMicroservice.Business.Model;

public record CardDetails(string CardNumber, CardType CardType, CardStatus CardStatus, bool IsPinSet);