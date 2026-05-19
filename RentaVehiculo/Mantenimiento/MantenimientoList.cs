using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Mantenimientos;

public partial class MantenimientoList : Form
{
    private readonly MantenimientoService _service;

    public MantenimientoList(MantenimientoService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void MantenimientoList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(MantenimientoListaFila.Id), MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Vehículo", DataPropertyName = nameof(MantenimientoListaFila.Vehiculo), MinimumWidth = 180, FillWeight = 130 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tipo", DataPropertyName = nameof(MantenimientoListaFila.TipoTexto), MinimumWidth = 120, FillWeight = 85 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Costo", DataPropertyName = nameof(MantenimientoListaFila.Costo), MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Inicio", DataPropertyName = nameof(MantenimientoListaFila.FechaInicio), MinimumWidth = 140, FillWeight = 100 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = nameof(MantenimientoListaFila.EstadoTexto), MinimumWidth = 120, FillWeight = 75 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private static MantenimientoListaFila MapearFila(Mantenimiento m)
    {
        var vehiculo = m.IdVehiculoNavigation is { } v
            ? $"{v.Marca} {v.Modelo} ({v.Placa})"
            : $"Id {m.IdVehiculo}";
        return new MantenimientoListaFila
        {
            Id = m.Id,
            Vehiculo = vehiculo,
            TipoTexto = MantenimientoTiposUi.NombreTipo(m.TipoMantenimiento),
            Costo = m.Costo,
            FechaInicio = m.FechaInicio,
            EstadoTexto = MantenimientoEstadosUi.NombreEstado(m.Estado)
        };
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetListConRelacionesAsync(m => true);
            dataGridView1.DataSource = list.Select(MapearFila).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<MantenimientoForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not MantenimientoListaFila fila)
        {
            MessageBox.Show("Seleccione un registro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró el mantenimiento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<MantenimientoForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not MantenimientoListaFila fila)
            return;
        if (MessageBox.Show("¿Eliminar este mantenimiento?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
