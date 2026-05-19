using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.Data.Models;
using System.Linq.Expressions;

namespace RentaVehiculo.UI.Services;

public class FacturaService(RentaVehiculosContext context) : IService<Factura, int>
{
    public Task<Factura?> Buscar(int id) =>
        context.Facturas.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

    public async Task<bool> Eliminar(int id)
    {
        var entity = await context.Facturas.FindAsync(id);
        if (entity is null)
            return false;
        context.Facturas.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Factura>> GetList(Expression<Func<Factura, bool>> criterio) =>
        await context.Facturas.AsNoTracking().Where(criterio).ToListAsync();

    /// <summary>Incluye renta, cliente y vehículo para mostrar nombres en listados.</summary>
    public async Task<List<Factura>> GetListConRelacionesAsync(Expression<Func<Factura, bool>> criterio, CancellationToken ct = default) =>
        await context.Facturas.AsNoTracking()
            .Include(f => f.IdRentaNavigation)
                .ThenInclude(r => r.IdClienteNavigation)
            .Include(f => f.IdRentaNavigation)
                .ThenInclude(r => r.IdVehiculoNavigation)
            .Where(criterio)
            .OrderByDescending(f => f.Id)
            .ToListAsync(ct);

    public async Task<bool> Insertar(Factura entity)
    {
        if (entity.FechaEmision == default)
            entity.FechaEmision = DateTime.Now;
        context.Facturas.Add(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Factura entity) =>
        !await Existe(entity.Id) ? await Insertar(entity) : await Modificar(entity);

    public Task<bool> Existe(int id) => context.Facturas.AnyAsync(f => f.Id == id);

    public async Task<bool> Modificar(Factura entity)
    {
        context.Facturas.Update(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
