namespace CardActionsApp.WebApi.Model;

public record CardActions(string UserId, string CardNumber, IEnumerable<string> AllowedActions);