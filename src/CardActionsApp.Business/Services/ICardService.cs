using CardActionsApp.WebApi.Model;

namespace CardActionsApp.WebApi.Services;

public interface ICardService
{
    Task<Dictionary<string, Dictionary<string, CardDetails>>> GetAllCardDetails();
    
    Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
}