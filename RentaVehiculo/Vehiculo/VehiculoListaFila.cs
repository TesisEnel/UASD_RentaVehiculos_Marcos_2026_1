namespace RentaVehiculo.UI.Vehiculos;

/// <summary>Fila de la rejilla de vehículos (estado legible).</summary>
public sealed class VehiculoListaFila
{
    public int Id { get; init; }
    public string Marca { get; init; } = "";
    public string Modelo { get; init; } = "";
    public int Año { get; init; }
    public string Placa { get; init; } = "";
    public int Kilometraje { get; init; }
    public decimal PrecioPorDia { get; init; }
    public string EstadoTexto { get; init; } = "";
}
