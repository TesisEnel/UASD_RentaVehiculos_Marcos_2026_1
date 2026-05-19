using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Facturas;

/// <summary>Valores de <see cref="RentaVehiculo.Data.Models.Factura.Estado"/> en la UI.</summary>
public static class FacturaEstadosUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Emitida" },
        new() { Id = 2, Nombre = "Pagada" },
        new() { Id = 3, Nombre = "Anulada" },
        new() { Id = 4, Nombre = "Vencida" }
    };

    public static string NombreEstado(int id)
    {
        foreach (var x in Predefinidos)
        {
            if (x.Id == id)
                return x.Nombre;
        }

        return $"Estado ({id})";
    }
}
