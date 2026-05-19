using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Services;

public class SeleccionCatalogoService(RentaVehiculosContext context)
{
    public async Task<List<ItemListaId>> ObtenerClientesAsync(CancellationToken ct = default)
    {
        var rows = await context.Clientes.AsNoTracking()
            .OrderBy(c => c.Nombre).ThenBy(c => c.Apellido)
            .Select(c => new { c.Id, c.Nombre, c.Apellido, c.Email })
            .ToListAsync(ct);
        return rows.Select(c => new ItemListaId
        {
            Id = c.Id,
            Nombre = $"{c.Nombre} {c.Apellido} — {c.Email}"
        }).ToList();
    }

    public async Task<List<ItemListaId>> ObtenerVehiculosAsync(bool soloActivos = true, CancellationToken ct = default)
    {
        var q = context.Vehiculos.AsNoTracking();
        if (soloActivos)
            q = q.Where(v => v.Activo);
        var rows = await q.OrderBy(v => v.Marca).ThenBy(v => v.Modelo)
            .Select(v => new { v.Id, v.Marca, v.Modelo, v.Placa })
            .ToListAsync(ct);
        return rows.Select(v => new ItemListaId
        {
            Id = v.Id,
            Nombre = $"{v.Marca} {v.Modelo} ({v.Placa})"
        }).ToList();
    }

    public async Task<List<ItemListaId>> ObtenerUsuariosActivosAsync(CancellationToken ct = default)
    {
        var rows = await context.Usuarios.AsNoTracking()
            .Where(u => u.Activo)
            .OrderBy(u => u.Nombre).ThenBy(u => u.Apellido)
            .Select(u => new { u.Id, u.Nombre, u.Apellido, u.NombreUsuario })
            .ToListAsync(ct);
        return rows.Select(u => new ItemListaId
        {
            Id = u.Id,
            Nombre = $"{u.Nombre} {u.Apellido} ({u.NombreUsuario})"
        }).ToList();
    }

    public async Task<List<ItemListaId>> ObtenerSucursalesActivasAsync(CancellationToken ct = default)
    {
        var rows = await context.Sucursales.AsNoTracking()
            .Where(s => s.Activa)
            .OrderBy(s => s.Nombre)
            .Select(s => new { s.Id, s.Nombre, s.Ciudad })
            .ToListAsync(ct);
        return rows.Select(s => new ItemListaId
        {
            Id = s.Id,
            Nombre = string.IsNullOrWhiteSpace(s.Ciudad) ? s.Nombre : $"{s.Nombre} ({s.Ciudad})"
        }).ToList();
    }

    public async Task<List<ItemListaId>> ObtenerRentasResumenAsync(int max = 400, CancellationToken ct = default)
    {
        var rows = await context.Rentas.AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdVehiculoNavigation)
            .OrderByDescending(r => r.Id)
            .Take(max)
            .ToListAsync(ct);
        return rows.Select(r => new ItemListaId
        {
            Id = r.Id,
            Nombre =
                $"Renta #{r.Id} — {r.IdClienteNavigation.Nombre} {r.IdClienteNavigation.Apellido} — {r.IdVehiculoNavigation.Marca} {r.IdVehiculoNavigation.Modelo} ({r.IdVehiculoNavigation.Placa})"
        }).ToList();
    }

    public async Task<List<string>> ObtenerProveedoresMantenimientoAsync(int max = 120, CancellationToken ct = default)
    {
        return await context.Mantenimientos.AsNoTracking()
            .Where(m => m.Proveedor != null && m.Proveedor != "")
            .Select(m => m.Proveedor!)
            .Distinct()
            .OrderBy(p => p)
            .Take(max)
            .ToListAsync(ct);
    }
}
