namespace CardActionsApp.WebApi.Model;

public record CardDetails(string CardNumber, CardType CardType, CardStatus CardStatus, bool IsPinSet);