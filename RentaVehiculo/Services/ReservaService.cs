using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.Data.Models;
using System.Linq.Expressions;

namespace RentaVehiculo.UI.Services;

public class ReservaService(RentaVehiculosContext context) : IService<Reserva, int>
{
    public Task<Reserva?> Buscar(int id) =>
        context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

    public async Task<bool> Eliminar(int id)
    {
        var entity = await context.Reservas.FindAsync(id);
        if (entity is null)
            return false;
        context.Reservas.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Reserva>> GetList(Expression<Func<Reserva, bool>> criterio) =>
        await context.Reservas.AsNoTracking().Where(criterio).ToListAsync();

    /// <summary>Incluye cliente y vehículo para mostrar nombres en listados.</summary>
    public async Task<List<Reserva>> GetListConRelacionesAsync(Expression<Func<Reserva, bool>> criterio, CancellationToken ct = default) =>
        await context.Reservas.AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdVehiculoNavigation)
            .Where(criterio)
            .OrderByDescending(r => r.Id)
            .ToListAsync(ct);

    public async Task<bool> Insertar(Reserva entity)
    {
        if (entity.FechaReserva == default)
            entity.FechaReserva = DateTime.Now;
        context.Reservas.Add(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Reserva entity) =>
        !await Existe(entity.Id) ? await Insertar(entity) : await Modificar(entity);

    public Task<bool> Existe(int id) => context.Reservas.AnyAsync(r => r.Id == id);

    public async Task<bool> Modificar(Reserva entity)
    {
        context.Reservas.Update(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
