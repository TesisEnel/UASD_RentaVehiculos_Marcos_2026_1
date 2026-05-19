using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Usuarios;

/// <summary>Roles del sistema y permisos por módulo.</summary>
public static class UsuarioRoles
{
    public const string Administrador = "Administrador";
    public const string Supervisor = "Supervisor";
    public const string Empleado = "Empleado";
    public const string Contador = "Contador";
    public const string Mecanico = "Mecánico";

    public static IReadOnlyList<string> Todos { get; } =
    [
        Administrador,
        Supervisor,
        Empleado,
        Contador,
        Mecanico
    ];

    private static readonly Dictionary<string, ModuloApp[]> PermisosPorRol =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Administrador] =
            [
                ModuloApp.Vehiculos, ModuloApp.Clientes, ModuloApp.Reservas, ModuloApp.Rentas,
                ModuloApp.Mantenimiento, ModuloApp.Facturacion, ModuloApp.Usuarios
            ],
            [Supervisor] =
            [
                ModuloApp.Vehiculos, ModuloApp.Clientes, ModuloApp.Reservas, ModuloApp.Rentas,
                ModuloApp.Mantenimiento, ModuloApp.Facturacion
            ],
            [Empleado] =
            [
                ModuloApp.Vehiculos, ModuloApp.Clientes, ModuloApp.Reservas, ModuloApp.Rentas
            ],
            [Contador] =
            [
                ModuloApp.Clientes, ModuloApp.Rentas, ModuloApp.Facturacion
            ],
            [Mecanico] =
            [
                ModuloApp.Vehiculos, ModuloApp.Mantenimiento
            ]
        };

    /// <summary>Unifica valores guardados en BD (p. ej. "Admin" → Administrador).</summary>
    public static string Normalizar(string? rol)
    {
        if (string.IsNullOrWhiteSpace(rol))
            return Empleado;

        var t = rol.Trim();
        if (t.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || t.Equals("Administrador", StringComparison.OrdinalIgnoreCase)
            || t.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
            return Administrador;

        foreach (var r in Todos)
        {
            if (t.Equals(r, StringComparison.OrdinalIgnoreCase))
                return r;
        }

        return t;
    }

    public static bool EsAdministrador(string? rol) =>
        Normalizar(rol) == Administrador;

    public static bool PuedeAcceder(string? rol, ModuloApp modulo)
    {
        var r = Normalizar(rol);
        if (!PermisosPorRol.TryGetValue(r, out var modulos))
            modulos = PermisosPorRol[Empleado];
        return modulos.Contains(modulo);
    }

    public static IReadOnlyList<ModuloApp> ModulosPermitidos(string? rol)
    {
        var r = Normalizar(rol);
        return PermisosPorRol.TryGetValue(r, out var modulos)
            ? modulos
            : PermisosPorRol[Empleado];
    }
}
