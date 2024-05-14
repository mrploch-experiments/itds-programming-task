using Itds.CardActionsMicroservice.Business.Model;

namespace Itds.CardActionsMicroservice.Business.Services;

public interface ICardService
{
    Task<CardDetails?> GetCardDetailsAsync(string userId, string cardNumber, CancellationToken cancellationToken = default);
}