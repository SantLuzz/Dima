using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class VouncherHandler(AppDbContext context) : IVouncherHandler
{
    public async Task<Response<Vouncher?>> GetByNumberAsync(GetVouncherByNumberRequest request)
    {
        try
        {
            var vouncher = await context
                .Vounchers
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Number == request.Number
                && o.IsActive);
            
            return vouncher is null
                ? new Response<Vouncher?>(null, 404, "Vouncher não encontrado.")
                : new Response<Vouncher?>((vouncher));
        }
        catch (Exception e)
        {
            return new Response<Vouncher?>(null, 500, "Não foi possível obter o vouncher.");
        }
    }
}