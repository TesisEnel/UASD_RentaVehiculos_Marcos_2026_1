namespace RentaVehiculo.UI.Rentas;

/// <summary>Fila de la rejilla de rentas (textos legibles para cliente, vehículo y estado).</summary>
public sealed class RentaListaFila
{
    public int Id { get; init; }
    public string Cliente { get; init; } = "";
    public string Vehiculo { get; init; } = "";
    public DateTime FechaInicio { get; init; }
    public DateTime FechaFinProgramada { get; init; }
    public string EstadoTexto { get; init; } = "";
    public decimal CostoTotal { get; init; }
}
