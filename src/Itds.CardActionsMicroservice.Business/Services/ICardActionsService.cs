using Itds.CardActionsMicroservice.Business.Model;

namespace Itds.CardActionsMicroservice.Business.Services;

public interface ICardActionsService
{
    Task<IEnumerable<string>> GetAllowedActionsAsync(CardDetails cardDetails, CancellationToken cancellationToken = default);
}