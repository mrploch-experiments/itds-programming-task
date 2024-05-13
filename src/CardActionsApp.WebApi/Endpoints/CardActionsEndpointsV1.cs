using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CardActionsApp.WebApi.Endpoints;

public static class CardActionsEndpointsV1
{
    public static RouteGroupBuilder MapCardActionsApiV1(this RouteGroupBuilder group)
    {
        group.WithOpenApi().WithTags("CardActions").MapGet("/", GetCardActions);

        return group;
    }
    
    public static async Task<Results<NotFound, BadRequest, Ok<CardActions>>> GetCardActions(string userId, string cardNumber, ICardService cardService, ICardActionsService cardActionsService)
    {
        var cardDetails = await cardService.GetCardDetails(userId, cardNumber);
        if (cardDetails is null)
        {
            return TypedResults.NotFound();
        }

        var allowedActions = await cardActionsService.GetAllowedActions(cardDetails);
        
        return TypedResults.Ok(new CardActions(userId, cardNumber, allowedActions));
    }
}