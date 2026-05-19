namespace RentaVehiculo.UI.Clientes;

/// <summary>Fila de la rejilla de clientes (texto legible para activo).</summary>
public sealed class ClienteListaFila
{
    public int Id { get; init; }
    public string Nombre { get; init; } = "";
    public string Apellido { get; init; } = "";
    public string Email { get; init; } = "";
    public string Telefono { get; init; } = "";
    public string Licencia { get; init; } = "";
    public string ActivoTexto { get; init; } = "";
}
