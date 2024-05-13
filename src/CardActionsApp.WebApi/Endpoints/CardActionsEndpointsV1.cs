using CardActionsApp.Business;
using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CardActionsApp.WebApi.Endpoints;

public static class CardActionsEndpointsV1
{
    public static RouteGroupBuilder MapCardActionsApiV1(this RouteGroupBuilder group)
    {
        var groupBuilder = group.WithOpenApi().WithTags("CardActions");
        groupBuilder.MapGet("/", GetCardActions);
        groupBuilder.MapGet("/carddetails", GetAllCardDetails);

        return group;
    }

    public static async Task<Results<NotFound, ProblemHttpResult, Ok<CardActions>>> GetCardActions(string userId, string cardNumber, GetAllowedCardActions getAllowedCardActions, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("CardActionsEndpointsV1");
        try
        {
            logger.LogDebug("Getting card actions for user {UserId} card {CardNumber}", userId, cardNumber);
            var result = await getAllowedCardActions.Execute(userId, cardNumber);
            
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
    
    public static async Task<Ok<Dictionary<string, Dictionary<string, CardDetails>>>> GetAllCardDetails(ICardService cardService)
    {
        var result = await cardService.GetAllCardDetails();
        return TypedResults.Ok(result);
    }
}