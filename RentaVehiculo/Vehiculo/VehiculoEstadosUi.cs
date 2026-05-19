using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Vehiculos;

/// <summary>Valores de <see cref="RentaVehiculo.Data.Models.Vehiculo.Estado"/> en la UI.</summary>
public static class VehiculoEstadosUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Disponible" },
        new() { Id = 2, Nombre = "Rentado" },
        new() { Id = 3, Nombre = "En mantenimiento" },
        new() { Id = 4, Nombre = "Fuera de servicio" },
        new() { Id = 5, Nombre = "Reservado" }
    };

    public static List<ItemListaId> ListaParaSeleccion(int? idActual = null)
    {
        var list = Predefinidos.ToList();
        if (idActual is > 0 && list.All(x => x.Id != idActual))
            list.Add(new ItemListaId { Id = idActual.Value, Nombre = $"Estado (código {idActual})" });
        return list;
    }

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
