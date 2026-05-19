using RentaVehiculo.UI.Infrastructure;

namespace RentaVehiculo.UI.Usuarios;

public static class UsuarioRolesUi
{
    public static List<ItemListaId> ListaRoles(string? rolActual = null)
    {
        var list = UsuarioRoles.Todos
            .Select((r, i) => new ItemListaId { Id = i + 1, Nombre = r })
            .ToList();

        var norm = UsuarioRoles.Normalizar(rolActual);
        if (!string.IsNullOrEmpty(rolActual)
            && !UsuarioRoles.Todos.Any(r => r.Equals(norm, StringComparison.OrdinalIgnoreCase)))
        {
            list.Add(new ItemListaId { Id = 99, Nombre = norm });
        }

        return list;
    }

    public static void EnlazarRoles(ComboBox cb, string? rolActual)
    {
        cb.DropDownStyle = ComboBoxStyle.DropDownList;
        cb.DisplayMember = nameof(ItemListaId.Nombre);
        cb.ValueMember = nameof(ItemListaId.Nombre);
        var items = ListaRoles(rolActual);
        cb.DataSource = items;
        var norm = UsuarioRoles.Normalizar(rolActual);
        try
        {
            cb.SelectedValue = norm;
        }
        catch
        {
            if (cb.Items.Count > 0)
                cb.SelectedIndex = 0;
        }
    }

    public static string RolSeleccionado(ComboBox cb)
    {
        if (cb.SelectedValue is string s && !string.IsNullOrWhiteSpace(s))
            return UsuarioRoles.Normalizar(s);
        if (cb.SelectedItem is ItemListaId x)
            return UsuarioRoles.Normalizar(x.Nombre);
        return UsuarioRoles.Empleado;
    }
}
