namespace RentaVehiculo.UI.Mantenimientos;

/// <summary>Fila de la rejilla de mantenimientos (textos legibles para vehículo, tipo y estado).</summary>
public sealed class MantenimientoListaFila
{
    public int Id { get; init; }
    public string Vehiculo { get; init; } = "";
    public string TipoTexto { get; init; } = "";
    public decimal Costo { get; init; }
    public DateTime FechaInicio { get; init; }
    public string EstadoTexto { get; init; } = "";
}
