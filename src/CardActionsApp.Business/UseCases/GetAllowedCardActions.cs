using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using FluentResults;
using Microsoft.Extensions.Logging;
using Polly;

namespace CardActionsApp.Business;

public class GetAllowedCardActions
{
    private readonly ICardService _cardService;
    private readonly ICardActionsService _cardActionsService;
    private readonly ILogger<GetAllowedCardActions> _logger;

    public GetAllowedCardActions(ICardService cardService, ICardActionsService cardActionsService, ILogger<GetAllowedCardActions> logger)
    {
        _cardService = cardService;
        _cardActionsService = cardActionsService;
        _logger = logger;
    }
    
    public async Task<CardActions> Execute(string userId, string cardNumber)
    {
        _logger.LogDebug("Retrieving card details for user {UserId} card {CardNumber}", userId, cardNumber);
        
        // Just as an example here - let's assume the services can intermittently fail
        var retryPolicy = Policy.Handle<Exception>(ex => !(ex is CardDetailsNotFoundException)).WaitAndRetryAsync(
                                                                                                                  retryCount: 3, 
                                                                                                                  sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                                                                                                                  onRetryAsync: async (exception, timespan, retryAttempt, context) =>
                                                                                                                                {
                                                                                                                                    _logger.LogWarning(exception,"Retry attempt {RetryAttempt} after {TimeSpanTotalSeconds} seconds due to: {ExceptionMessage}", retryAttempt, timespan, exception.Message);
                                                                                                                                });
        
        var cardDetails = await retryPolicy.ExecuteAsync(async () => await _cardService.GetCardDetails(userId, cardNumber));
        if (cardDetails is null)
        {
            _logger.LogError("Card details for user {UserId} card {CardNumber} not found", userId, cardNumber);

            throw new CardDetailsNotFoundException(userId, cardNumber, $"Card details for user {userId} card {cardNumber} not found");
        }
        
        var allowedActions = await retryPolicy.ExecuteAsync(async () => await _cardActionsService.GetAllowedActions(cardDetails));

        return new CardActions(userId, cardNumber, allowedActions);
    }
}