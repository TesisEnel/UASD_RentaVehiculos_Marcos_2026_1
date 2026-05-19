using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Mantenimientos;

/// <summary>Valores de <see cref="RentaVehiculo.Data.Models.Mantenimiento.Estado"/> en la UI.</summary>
public static class MantenimientoEstadosUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Programado" },
        new() { Id = 2, Nombre = "En ejecución" },
        new() { Id = 3, Nombre = "Completado" },
        new() { Id = 4, Nombre = "Cancelado" },
        new() { Id = 5, Nombre = "Pendiente de repuestos" }
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
