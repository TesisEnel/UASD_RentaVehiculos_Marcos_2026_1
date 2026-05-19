using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Vehiculos;

public partial class VehiculoList : Form
{
    private readonly VehiculoService _service;

    public VehiculoList(VehiculoService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void VehiculoList_Load(object sender, EventArgs e)
    {
        ConfigureColumns();
        _ = LoadDataAsync();
    }

    private void ConfigureColumns()
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(VehiculoListaFila.Id), MinimumWidth = 56, FillWeight = 40 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Marca", DataPropertyName = nameof(VehiculoListaFila.Marca), MinimumWidth = 90, FillWeight = 90 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Modelo", DataPropertyName = nameof(VehiculoListaFila.Modelo), MinimumWidth = 90, FillWeight = 90 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Año", DataPropertyName = nameof(VehiculoListaFila.Año), MinimumWidth = 64, FillWeight = 50 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Placa", DataPropertyName = nameof(VehiculoListaFila.Placa), MinimumWidth = 88, FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Km", DataPropertyName = nameof(VehiculoListaFila.Kilometraje), MinimumWidth = 72, FillWeight = 55 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Precio/día", DataPropertyName = nameof(VehiculoListaFila.PrecioPorDia), MinimumWidth = 88, FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = nameof(VehiculoListaFila.EstadoTexto), MinimumWidth = 120, FillWeight = 75 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
    }

    private static VehiculoListaFila MapearFila(Vehiculo v) => new()
    {
        Id = v.Id,
        Marca = v.Marca,
        Modelo = v.Modelo,
        Año = v.Año,
        Placa = v.Placa,
        Kilometraje = v.Kilometraje,
        PrecioPorDia = v.PrecioPorDia,
        EstadoTexto = VehiculoEstadosUi.NombreEstado(v.Estado)
    };

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetList(v => true);
            dataGridView1.DataSource = list.Select(MapearFila).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var form = Program.ServiceProvider.GetRequiredService<VehiculoForm>();
        if (form.ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not VehiculoListaFila fila)
        {
            MessageBox.Show("Seleccione un vehículo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró el vehículo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var form = ActivatorUtilities.CreateInstance<VehiculoForm>(Program.ServiceProvider, entidad);
        if (form.ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not VehiculoListaFila fila)
        {
            MessageBox.Show("Seleccione un vehículo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"¿Eliminar vehículo {fila.Placa}?", "Confirmar", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = EliminarAsync(fila.Id);
    }

    private async Task EliminarAsync(int id)
    {
        try
        {
            if (await _service.Eliminar(id))
            {
                MessageBox.Show("Eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
            else
                MessageBox.Show("No se pudo eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
