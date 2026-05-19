using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.Data.Models;
using System.Linq.Expressions;

namespace RentaVehiculo.UI.Services;

public class MantenimientoService(RentaVehiculosContext context) : IService<Mantenimiento, int>
{
    public Task<Mantenimiento?> Buscar(int id) =>
        context.Mantenimientos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

    public async Task<bool> Eliminar(int id)
    {
        var entity = await context.Mantenimientos.FindAsync(id);
        if (entity is null)
            return false;
        context.Mantenimientos.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Mantenimiento>> GetList(Expression<Func<Mantenimiento, bool>> criterio) =>
        await context.Mantenimientos.AsNoTracking().Where(criterio).ToListAsync();

    /// <summary>Incluye vehículo para mostrar nombre en listados.</summary>
    public async Task<List<Mantenimiento>> GetListConRelacionesAsync(Expression<Func<Mantenimiento, bool>> criterio, CancellationToken ct = default) =>
        await context.Mantenimientos.AsNoTracking()
            .Include(m => m.IdVehiculoNavigation)
            .Where(criterio)
            .OrderByDescending(m => m.Id)
            .ToListAsync(ct);

    public async Task<bool> Insertar(Mantenimiento entity)
    {
        context.Mantenimientos.Add(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Mantenimiento entity) =>
        !await Existe(entity.Id) ? await Insertar(entity) : await Modificar(entity);

    public Task<bool> Existe(int id) => context.Mantenimientos.AnyAsync(m => m.Id == id);

    public async Task<bool> Modificar(Mantenimiento entity)
    {
        context.Mantenimientos.Update(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
