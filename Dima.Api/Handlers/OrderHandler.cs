using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class OrderHandler(AppDbContext context) : IOrderHandler
{
    public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        Order? order = null;
        try
        {
            order = await context
                .Orders
                .Include(x => x.Product)
                .Include(x => x.Vouncher)
                .FirstOrDefaultAsync(x => x.Id == request.Id 
                && x .UserId == request.UserId);
                
            if(order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado");
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao obter o pedido");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Este pedido já foi cancelado.");
            case EOrderStatus.WaitingPayment: break;
            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "Este pedido já foi pago, e não pode ser cancelado.");
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Este pedido já foi rembolsado e não pode ser cancelado.");
            default:
                return new Response<Order?>(order, 400, "Este pedido não pode ser cancelado.");
        }
        
        order.Status = EOrderStatus.Canceled;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(order, 500, "Não foi possível cancelar o pedido");
        }

        return new Response<Order?>(order, 200, $"Pedido {order.Number} cancelado com sucesso.");
    }

    public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        Product? product = null;
        Vouncher? vouncher = null;

        try
        {
            product = await context
                .Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProductId
                && x.IsActive);

            if (product is null)
                return new Response<Order?>(null, 404, "Produto não encontrado");
            
            context.Add(product);
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao obter o produto");
        }

        try
        {
            if (request.VouncherId is not null)
            {
                vouncher = await context
                    .Vounchers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.VouncherId
                                              && x.IsActive);

                if (vouncher is null)
                    return new Response<Order?>(null, 404, "Vouncher inválido ou não encontrado");
                if(!vouncher.IsActive)
                    return new Response<Order?>(null, 404, "Vouncher já foi utilizado");

                vouncher.IsActive = false;
                context.Vounchers.Update(vouncher);
            }
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao obter o vouncher informado");
        }


        var order = new Order
        {
            UserId = request.UserId,
            Product = product,
            ProductId = request.ProductId,
            Vouncher = vouncher,
            VouncherId = request.VouncherId,
        };

        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(null, 500, "Falha ao realizar o pedido");
        }

        return new Response<Order?>(order, 201, $"Pedido {order.Number} cadastrado com sucesso.");
    }

    public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        Order? order = null;
        try
        {
            order = await context
                .Orders
                .FirstOrDefaultAsync(x => x.Id == request.Id
                && x.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado.");
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao obter o pedido");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Pedido já cancelado.");
            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "Pedido já foi pago.");
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Pedido já foi reembolsado.");
            case EOrderStatus.WaitingPayment: break;
            default:
            return new Response<Order?>(order, 400, "Não foi possível pagar o pedido.");
        }
        
        order.Status = EOrderStatus.Refunded;
        order.ExternalReference = request.ExternalReference;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync(); 
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao pagar o pedido");
        }

        return new Response<Order?>(order, 200, $"Pedido {order.Number} pago com sucesso.");
    }

    public Task<Response<Order?>> RefoundAsync(RefoundOrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        throw new NotImplementedException();
    }
}