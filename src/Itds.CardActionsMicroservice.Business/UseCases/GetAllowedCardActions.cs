using Itds.CardActionsMicroservice.Business.Model;
using Itds.CardActionsMicroservice.Business.Services;
using Microsoft.Extensions.Logging;
using Polly;

namespace Itds.CardActionsMicroservice.Business.UseCases;

public class GetAllowedCardActions(ICardService cardService, ICardActionsService cardActionsService, ILogger<GetAllowedCardActions> logger)
{
    public async Task<CardActions> ExecuteAsync(string userId, string cardNumber, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Retrieving card details for user {UserId} card {CardNumber}", userId, cardNumber);

        // Just as an example here - let's assume the services can intermittently fail
        var retryPolicy = Policy.Handle<Exception>(ex => ex is not CardDetailsNotFoundException)
                                .WaitAndRetryAsync(3,
                                                   static retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                   (exception, timespan, retryAttempt, _) =>
                                                   {
                                                       logger.LogWarning(exception,
                                                                         "Retry attempt {RetryAttempt} after {TimeSpanTotalSeconds} seconds due to: {ExceptionMessage}",
                                                                         retryAttempt,
                                                                         timespan,
                                                                         exception.Message);
                                                   });

        var cardDetails = await retryPolicy.ExecuteAsync(async () => await cardService.GetCardDetailsAsync(userId, cardNumber, cancellationToken));
        if (cardDetails is null)
        {
            logger.LogError("Card details for user {UserId} card {CardNumber} not found", userId, cardNumber);

            throw new CardDetailsNotFoundException(userId, cardNumber, $"Card details for user {userId} card {cardNumber} not found");
        }

        var allowedActions = await retryPolicy.ExecuteAsync(async () => await cardActionsService.GetAllowedActionsAsync(cardDetails, cancellationToken));

        return new CardActions(userId, cardNumber, allowedActions);
    }
}