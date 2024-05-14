namespace Itds.CardActionsMicroservice.Business.Model;

public record CardActions(string UserId, string CardNumber, IEnumerable<string> AllowedActions);