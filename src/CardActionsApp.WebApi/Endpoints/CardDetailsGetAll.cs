using CardActionsApp.WebApi.Services;

namespace CardActionsApp.WebApi.Endpoints;

public static class CardDetailsGetAll
{
    public static async Task<IResult> Execute(ICardService cardService)
    {
        return TypedResults.Ok(await cardService.GetAllCardDetails());
    }
}