namespace RentaVehiculo.UI.Infrastructure;

public static class ComboSeleccion
{
    public static void Enlazar(ComboBox cb, IList<ItemListaId> items, int? idSeleccionado)
    {
        cb.DropDownStyle = ComboBoxStyle.DropDownList;
        cb.DisplayMember = nameof(ItemListaId.Nombre);
        cb.ValueMember = nameof(ItemListaId.Id);
        cb.DataSource = items.ToList();
        if (idSeleccionado is > 0)
        {
            try
            {
                cb.SelectedValue = idSeleccionado.Value;
            }
            catch
            {
                if (cb.Items.Count > 0)
                    cb.SelectedIndex = 0;
            }
        }
        else if (cb.Items.Count > 0)
            cb.SelectedIndex = 0;
    }

    public static int IdSeleccionado(ComboBox cb)
    {
        if (cb.SelectedItem is ItemListaId x)
            return x.Id;
        if (cb.SelectedValue is int i)
            return i;
        return 0;
    }

    /// <summary>Combo editable con sugerencias de proveedores (permite escribir uno nuevo).</summary>
    public static void CargarProveedoresSugeridos(ComboBox cbo, IEnumerable<string> sugeridos, string? valorActual)
    {
        cbo.DropDownStyle = ComboBoxStyle.DropDown;
        cbo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cbo.AutoCompleteSource = AutoCompleteSource.ListItems;
        cbo.Items.Clear();
        foreach (var p in sugeridos)
        {
            var t = p.Trim();
            if (t.Length == 0)
                continue;
            if (!cbo.Items.Contains(t))
                cbo.Items.Add(t);
        }

        if (!string.IsNullOrWhiteSpace(valorActual))
        {
            var act = valorActual.Trim();
            if (!cbo.Items.Contains(act))
                cbo.Items.Insert(0, act);
            cbo.Text = act;
        }
    }
}
