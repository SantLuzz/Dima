namespace Dima.Core.Requests.Orders;

public class GetVouncherByNumberRequest : Request
{
    public string Number { get; set; } = string.Empty;
}