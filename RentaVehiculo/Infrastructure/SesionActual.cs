using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Usuarios;

namespace RentaVehiculo.UI.Infrastructure;

/// <summary>Usuario autenticado en la sesión de la aplicación de escritorio.</summary>
public static class SesionActual
{
    public static Usuario? Usuario { get; private set; }

    public static bool EstaAutenticado => Usuario is not null;

    public static void Iniciar(Usuario usuario) => Usuario = usuario;

    public static void Cerrar() => Usuario = null;

    public static string Rol => Usuario is null ? "" : UsuarioRoles.Normalizar(Usuario.Rol);

    public static bool PuedeAcceder(ModuloApp modulo) =>
        Usuario is not null && UsuarioRoles.PuedeAcceder(Usuario.Rol, modulo);

    public static bool EsAdministrador => UsuarioRoles.EsAdministrador(Usuario?.Rol);
}
