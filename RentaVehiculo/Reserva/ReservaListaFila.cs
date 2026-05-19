namespace RentaVehiculo.UI.Reservas;

/// <summary>Fila de la rejilla de reservas (textos legibles para cliente, vehículo y estado).</summary>
public sealed class ReservaListaFila
{
    public int Id { get; init; }
    public string Cliente { get; init; } = "";
    public string Vehiculo { get; init; } = "";
    public DateTime FechaInicioReserva { get; init; }
    public DateTime FechaFinReserva { get; init; }
    public string EstadoTexto { get; init; } = "";
    public decimal MontoDeposito { get; init; }
    public bool DepositoPagado { get; init; }
}
