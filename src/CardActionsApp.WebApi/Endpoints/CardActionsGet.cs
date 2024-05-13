using CardActionsApp.WebApi.Model;
using CardActionsApp.WebApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CardActionsApp.WebApi.Endpoints;

public static class CardActionsGet
{
    public static async Task<Results<NotFound, BadRequest, Ok<CardActions>>> Execute(string userId, string cardNumber, ICardService cardService)
    {
        var cardDetails = await cardService.GetCardDetails(userId, cardNumber);
        if (cardDetails is null)
        {
            return TypedResults.NotFound();
        }

        if (cardDetails.CardType == CardType.Prepaid && cardDetails.CardStatus == CardStatus.Closed)
        {
            return TypedResults.Ok(new CardActions(userId, cardNumber, ["ACTION3", "ACTION4", "ACTION9"]));
        }
        
        if (cardDetails.CardType == CardType.Credit && cardDetails.CardStatus == CardStatus.Blocked)
        {
            return TypedResults.Ok(new CardActions(userId, cardNumber, ["ACTION3", "ACTION4", "ACTION3", "ACTION6"]));
        }

        return TypedResults.BadRequest();
    }
}