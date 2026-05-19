using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using System.Linq.Expressions;
using UsuarioEntity = RentaVehiculo.Data.Models.Usuario;

namespace RentaVehiculo.UI.Services;

public class UsuarioService(RentaVehiculosContext context) : IService<UsuarioEntity, int>
{
    public Task<UsuarioEntity?> Buscar(int id)
    {
        return context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        var usuario = await context.Usuarios.FindAsync(id);
        if (usuario is null)
            return false;

        context.Usuarios.Remove(usuario);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<UsuarioEntity>> GetList(Expression<Func<UsuarioEntity, bool>> criterio)
    {
        return await context.Usuarios
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }

    public async Task<bool> Insertar(UsuarioEntity usuario)
    {
        usuario.FechaCreacion = DateTime.Now;
        context.Usuarios.Add(usuario);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(UsuarioEntity usuario)
    {
        if (!await Existe(usuario.Id))
            return await Insertar(usuario);
        else
            return await Modificar(usuario);
    }

    public async Task<bool> Existe(int id)
    {
        return await context.Usuarios.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> Modificar(UsuarioEntity usuario)
    {
        context.Usuarios.Update(usuario);
        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>Valida credenciales. La contraseña se guarda en texto plano (coherente con el formulario de usuarios).</summary>
    public async Task<UsuarioEntity?> ValidarInicioSesionAsync(string nombreUsuario, string password)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
            return null;

        var clave = nombreUsuario.Trim();
        var pass = password.Trim();

        var candidatos = await context.Usuarios.AsNoTracking()
            .Where(u => u.Activo)
            .ToListAsync();

        var usuario = candidatos.FirstOrDefault(u =>
            string.Equals(u.NombreUsuario, clave, StringComparison.OrdinalIgnoreCase));

        if (usuario is null)
            return null;

        if (!CoincideContrasena(usuario.PasswordHash, pass))
            return null;

        return usuario;
    }

    private static bool CoincideContrasena(string almacenada, string ingresada)
    {
        if (string.Equals(almacenada, ingresada, StringComparison.Ordinal))
            return true;

        // Compatibilidad con registros demo antiguos en BD
        if (string.Equals(almacenada, "$2a$11$DEMO_HASH_NO_USAR_EN_PRODUCCION", StringComparison.Ordinal)
            && string.Equals(ingresada, "cambiar123", StringComparison.Ordinal))
            return true;

        return false;
    }
}
