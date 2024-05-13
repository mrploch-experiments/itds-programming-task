using CardActionsApp.WebApi.Model;

namespace CardActionsApp.WebApi.Services;

public interface ICardActionsService
{
    Task<IEnumerable<string>> GetAllowedActions(CardDetails cardDetails);
}