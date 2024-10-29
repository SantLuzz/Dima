using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Core.Handlers;

public interface IVouncherHandler
{
    Task<Response<Vouncher?>> GetByNumberAsync(GetVouncherByNumberRequest request);
}