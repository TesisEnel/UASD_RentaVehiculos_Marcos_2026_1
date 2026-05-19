using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Reservas;

public partial class ReservaList : Form
{
    private readonly ReservaService _service;

    public ReservaList(ReservaService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void ReservaList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(ReservaListaFila.Id), MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cliente", DataPropertyName = nameof(ReservaListaFila.Cliente), MinimumWidth = 160, FillWeight = 120 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Vehículo", DataPropertyName = nameof(ReservaListaFila.Vehiculo), MinimumWidth = 180, FillWeight = 130 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Inicio", DataPropertyName = nameof(ReservaListaFila.FechaInicioReserva), MinimumWidth = 130, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fin", DataPropertyName = nameof(ReservaListaFila.FechaFinReserva), MinimumWidth = 130, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = nameof(ReservaListaFila.EstadoTexto), MinimumWidth = 120, FillWeight = 75 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Depósito", DataPropertyName = nameof(ReservaListaFila.MontoDeposito), MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Dep. pagado", DataPropertyName = nameof(ReservaListaFila.DepositoPagado), MinimumWidth = 100, FillWeight = 65 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private static ReservaListaFila MapearFila(Reserva r)
    {
        var cliente = r.IdClienteNavigation is { } c
            ? $"{c.Nombre} {c.Apellido}".Trim()
            : $"Id {r.IdCliente}";
        var vehiculo = r.IdVehiculoNavigation is { } v
            ? $"{v.Marca} {v.Modelo} ({v.Placa})"
            : $"Id {r.IdVehiculo}";
        return new ReservaListaFila
        {
            Id = r.Id,
            Cliente = string.IsNullOrWhiteSpace(cliente) ? $"Cliente #{r.IdCliente}" : cliente,
            Vehiculo = vehiculo,
            FechaInicioReserva = r.FechaInicioReserva,
            FechaFinReserva = r.FechaFinReserva,
            EstadoTexto = ReservaEstadosUi.NombreEstado(r.Estado),
            MontoDeposito = r.MontoDeposito,
            DepositoPagado = r.DepositoPagado
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
        if (Program.ServiceProvider.GetRequiredService<ReservaForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not ReservaListaFila fila)
        {
            MessageBox.Show("Seleccione una reserva.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró la reserva.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<ReservaForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not ReservaListaFila fila)
            return;
        if (MessageBox.Show("¿Eliminar esta reserva?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
