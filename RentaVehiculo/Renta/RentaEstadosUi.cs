using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Rentas;

/// <summary>Valores de <see cref="Data.Models.Renta.Estado"/> mostrados en la UI (catálogo fijo).</summary>
public static class RentaEstadosUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Activa" },
        new() { Id = 2, Nombre = "En curso" },
        new() { Id = 3, Nombre = "Finalizada" },
        new() { Id = 4, Nombre = "Cancelada" },
        new() { Id = 5, Nombre = "Pendiente de confirmación" }
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
