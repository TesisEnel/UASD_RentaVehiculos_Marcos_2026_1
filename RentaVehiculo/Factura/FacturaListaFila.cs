namespace RentaVehiculo.UI.Facturas;

/// <summary>Fila de la rejilla de facturas (textos legibles para renta y estado).</summary>
public sealed class FacturaListaFila
{
    public int Id { get; init; }
    public string Renta { get; init; } = "";
    public string NumeroFactura { get; init; } = "";
    public DateTime FechaEmision { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Impuestos { get; init; }
    public decimal Total { get; init; }
    public string EstadoTexto { get; init; } = "";
}
