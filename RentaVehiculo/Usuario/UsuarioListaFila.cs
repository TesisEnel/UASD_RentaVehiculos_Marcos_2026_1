namespace RentaVehiculo.UI.Usuarios;

public sealed class UsuarioListaFila
{
    public int Id { get; init; }
    public string Nombre { get; init; } = "";
    public string Apellido { get; init; } = "";
    public string NombreUsuario { get; init; } = "";
    public string Email { get; init; } = "";
    public string RolTexto { get; init; } = "";
    public string ActivoTexto { get; init; } = "";
}
