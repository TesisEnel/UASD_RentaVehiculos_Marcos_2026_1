using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Reservas;

/// <summary>Valores de <see cref="RentaVehiculo.Data.Models.Reserva.Estado"/> en la UI.</summary>
public static class ReservaEstadosUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Pendiente" },
        new() { Id = 2, Nombre = "Confirmada" },
        new() { Id = 3, Nombre = "En curso" },
        new() { Id = 4, Nombre = "Completada" },
        new() { Id = 5, Nombre = "Cancelada" },
        new() { Id = 6, Nombre = "No presentado" }
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
