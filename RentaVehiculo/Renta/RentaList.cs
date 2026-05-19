using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Rentas;

public partial class RentaList : Form
{
    private readonly RentaService _service;

    public RentaList(RentaService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void RentaList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(RentaListaFila.Id), MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cliente", DataPropertyName = nameof(RentaListaFila.Cliente), MinimumWidth = 160, FillWeight = 120 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Vehículo", DataPropertyName = nameof(RentaListaFila.Vehiculo), MinimumWidth = 180, FillWeight = 130 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Inicio", DataPropertyName = nameof(RentaListaFila.FechaInicio), MinimumWidth = 130, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fin prog.", DataPropertyName = nameof(RentaListaFila.FechaFinProgramada), MinimumWidth = 130, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = nameof(RentaListaFila.EstadoTexto), MinimumWidth = 120, FillWeight = 75 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Costo total", DataPropertyName = nameof(RentaListaFila.CostoTotal), MinimumWidth = 96, FillWeight = 75 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private static RentaListaFila MapearFila(Renta r)
    {
        var cliente = r.IdClienteNavigation is { } c
            ? $"{c.Nombre} {c.Apellido}".Trim()
            : $"Id {r.IdCliente}";
        var vehiculo = r.IdVehiculoNavigation is { } v
            ? $"{v.Marca} {v.Modelo} ({v.Placa})"
            : $"Id {r.IdVehiculo}";
        return new RentaListaFila
        {
            Id = r.Id,
            Cliente = string.IsNullOrWhiteSpace(cliente) ? $"Cliente #{r.IdCliente}" : cliente,
            Vehiculo = vehiculo,
            FechaInicio = r.FechaInicio,
            FechaFinProgramada = r.FechaFinProgramada,
            EstadoTexto = RentaEstadosUi.NombreEstado(r.Estado),
            CostoTotal = r.CostoTotal
        };
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetListConRelacionesAsync(r => true);
            dataGridView1.DataSource = list.Select(MapearFila).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<RentaForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not RentaListaFila fila)
        {
            MessageBox.Show("Seleccione una renta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró la renta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<RentaForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not RentaListaFila fila)
            return;
        if (MessageBox.Show("¿Eliminar esta renta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
