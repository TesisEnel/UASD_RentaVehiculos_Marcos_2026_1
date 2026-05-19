using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.Data.Models;
using System.Linq.Expressions;

namespace RentaVehiculo.UI.Services;

public class RentaService(RentaVehiculosContext context) : IService<Renta, int>
{
    public Task<Renta?> Buscar(int id) =>
        context.Rentas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

    public async Task<bool> Eliminar(int id)
    {
        var entity = await context.Rentas.FindAsync(id);
        if (entity is null)
            return false;
        context.Rentas.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Renta>> GetList(Expression<Func<Renta, bool>> criterio) =>
        await context.Rentas.AsNoTracking().Where(criterio).ToListAsync();

    /// <summary>Incluye cliente y vehículo para mostrar nombres en listados.</summary>
    public async Task<List<Renta>> GetListConRelacionesAsync(Expression<Func<Renta, bool>> criterio, CancellationToken ct = default) =>
        await context.Rentas.AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdVehiculoNavigation)
            .Where(criterio)
            .OrderByDescending(r => r.Id)
            .ToListAsync(ct);

    public async Task<bool> Insertar(Renta entity)
    {
        if (entity.FechaCreacion == default)
            entity.FechaCreacion = DateTime.Now;
        context.Rentas.Add(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Renta entity) =>
        !await Existe(entity.Id) ? await Insertar(entity) : await Modificar(entity);

    public Task<bool> Existe(int id) => context.Rentas.AnyAsync(r => r.Id == id);

    public async Task<bool> Modificar(Renta entity)
    {
        context.Rentas.Update(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
