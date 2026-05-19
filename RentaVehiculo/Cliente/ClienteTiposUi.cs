using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Clientes;

/// <summary>Valores de <see cref="RentaVehiculo.Data.Models.Cliente.TipoCliente"/> en la UI.</summary>
public static class ClienteTiposUi
{
    private static readonly ItemListaId[] Predefinidos =
    {
        new() { Id = 1, Nombre = "Particular" },
        new() { Id = 2, Nombre = "Corporativo" },
        new() { Id = 3, Nombre = "VIP" },
        new() { Id = 4, Nombre = "Cliente frecuente" },
        new() { Id = 5, Nombre = "Gubernamental / institucional" }
    };

    public static List<ItemListaId> ListaParaSeleccion(int? idActual = null)
    {
        var list = Predefinidos.ToList();
        if (idActual is > 0 && list.All(x => x.Id != idActual))
            list.Add(new ItemListaId { Id = idActual.Value, Nombre = $"Tipo (código {idActual})" });
        return list;
    }
}
