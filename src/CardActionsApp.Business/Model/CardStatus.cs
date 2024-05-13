namespace CardActionsApp.WebApi.Model;

[Flags]
public enum CardStatus
{
    Ordered = 1,
    Inactive = 2,
    Active = 4,
    Restricted = 8,
    Blocked = 16,
    Expired = 32,
    Closed = 64,
    PinSet = 128,
    PinUnset = 256
}