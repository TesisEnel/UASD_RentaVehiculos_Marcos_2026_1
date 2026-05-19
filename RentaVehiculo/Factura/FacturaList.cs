using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Facturas;

public partial class FacturaList : Form
{
    private readonly FacturaService _service;

    public FacturaList(FacturaService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void FacturaList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(FacturaListaFila.Id), MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Renta", DataPropertyName = nameof(FacturaListaFila.Renta), MinimumWidth = 280, FillWeight = 160 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Número", DataPropertyName = nameof(FacturaListaFila.NumeroFactura), MinimumWidth = 120, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = nameof(FacturaListaFila.FechaEmision), MinimumWidth = 130, FillWeight = 90 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Subtotal", DataPropertyName = nameof(FacturaListaFila.Subtotal), MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Impuestos", DataPropertyName = nameof(FacturaListaFila.Impuestos), MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = nameof(FacturaListaFila.Total), MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = nameof(FacturaListaFila.EstadoTexto), MinimumWidth = 100, FillWeight = 70 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private static FacturaListaFila MapearFila(Factura f)
    {
        return new FacturaListaFila
        {
            Id = f.Id,
            Renta = TextoRenta(f),
            NumeroFactura = f.NumeroFactura,
            FechaEmision = f.FechaEmision,
            Subtotal = f.Subtotal,
            Impuestos = f.Impuestos,
            Total = f.Total,
            EstadoTexto = FacturaEstadosUi.NombreEstado(f.Estado)
        };
    }

    private static string TextoRenta(Factura f)
    {
        if (f.IdRentaNavigation is not { } r)
            return $"Renta #{f.IdRenta}";

        var cliente = r.IdClienteNavigation is { } c
            ? $"{c.Nombre} {c.Apellido}".Trim()
            : $"Cliente #{r.IdCliente}";
        if (string.IsNullOrWhiteSpace(cliente))
            cliente = $"Cliente #{r.IdCliente}";

        var vehiculo = r.IdVehiculoNavigation is { } v
            ? $"{v.Marca} {v.Modelo} ({v.Placa})"
            : $"Vehículo #{r.IdVehiculo}";

        return $"Renta #{r.Id} — {cliente} — {vehiculo}";
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetListConRelacionesAsync(f => true);
            dataGridView1.DataSource = list.Select(MapearFila).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<FacturaForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not FacturaListaFila fila)
        {
            MessageBox.Show("Seleccione una factura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró la factura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<FacturaForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not FacturaListaFila fila)
            return;
        if (MessageBox.Show("¿Eliminar esta factura?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _ = EliminarAsync(fila.Id);
    }

    private async Task EliminarAsync(int id)
    {
        try
        {
            if (await _service.Eliminar(id))
            {
                MessageBox.Show("Eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
