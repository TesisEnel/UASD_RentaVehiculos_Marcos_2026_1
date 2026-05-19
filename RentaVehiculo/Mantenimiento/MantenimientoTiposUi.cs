using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Mantenimientos;

/// <summary>Valores de <see cref="RentaVehiculo.Data.Models.Mantenimiento.TipoMantenimiento"/> en la UI.</summary>
public static class MantenimientoTiposUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Preventivo" },
        new() { Id = 2, Nombre = "Correctivo" },
        new() { Id = 3, Nombre = "Predictivo" },
        new() { Id = 4, Nombre = "Emergencia" },
        new() { Id = 5, Nombre = "Inspección / revisión" }
    };

    public static List<ItemListaId> ListaParaSeleccion(int? idActual = null)
    {
        var list = Predefinidos.ToList();
        if (idActual is > 0 && list.All(x => x.Id != idActual))
            list.Add(new ItemListaId { Id = idActual.Value, Nombre = $"Tipo (código {idActual})" });
        return list;
    }

    public static string NombreTipo(int id)
    {
        foreach (var x in Predefinidos)
        {
            if (x.Id == id)
                return x.Nombre;
        }

        return $"Tipo ({id})";
    }
}
