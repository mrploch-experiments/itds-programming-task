using Itds.CardActionsMicroservice.Business;
using Itds.CardActionsMicroservice.Business.Model;
using Itds.CardActionsMicroservice.Business.UseCases;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Itds.CardActionsMicroservice.WebApi.Endpoints;

public static class CardActionsEndpointsV1
{
    public static RouteGroupBuilder MapCardActionsApiV1(this RouteGroupBuilder group)
    {
        var groupBuilder = group.WithOpenApi().WithTags("CardActions");
        groupBuilder.MapGet("/", GetCardActionsAsync);

        return group;
    }

    private static async Task<Results<NotFound, ProblemHttpResult, Ok<CardActions>>> GetCardActionsAsync(string userId,
                                                                                                         string cardNumber,
                                                                                                         GetAllowedCardActions getAllowedCardActions,
                                                                                                         ILoggerFactory loggerFactory,
                                                                                                         CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger("CardActionsEndpointsV1");
        try
        {
            logger.LogDebug("Getting card actions for user {UserId} card {CardNumber}", userId, cardNumber);
            var result = await getAllowedCardActions.ExecuteAsync(userId, cardNumber, cancellationToken);

            return TypedResults.Ok(result);
        }
        catch (CardDetailsNotFoundException ex)
        {
            logger.LogError(ex, "Card details not found for user {UserId} card {CardNumber}", userId, cardNumber);

            return TypedResults.NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting card actions for user {UserId} card {CardNumber}", userId, cardNumber);

            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}