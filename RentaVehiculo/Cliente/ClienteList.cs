using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Clientes;

public partial class ClienteList : Form
{
    private readonly ClienteService _service;

    public ClienteList(ClienteService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void ClienteList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(ClienteListaFila.Id), MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = nameof(ClienteListaFila.Nombre), MinimumWidth = 100, FillWeight = 85 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Apellido", DataPropertyName = nameof(ClienteListaFila.Apellido), MinimumWidth = 100, FillWeight = 85 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = nameof(ClienteListaFila.Email), MinimumWidth = 160, FillWeight = 120 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Teléfono", DataPropertyName = nameof(ClienteListaFila.Telefono), MinimumWidth = 100, FillWeight = 75 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Licencia", DataPropertyName = nameof(ClienteListaFila.Licencia), MinimumWidth = 100, FillWeight = 75 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Activo", DataPropertyName = nameof(ClienteListaFila.ActivoTexto), MinimumWidth = 80, FillWeight = 55 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private static ClienteListaFila MapearFila(Cliente c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        Email = c.Email,
        Telefono = c.Telefono,
        Licencia = c.Licencia,
        ActivoTexto = c.Activo ? "Sí" : "No"
    };

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetList(c => true);
            dataGridView1.DataSource = list.Select(MapearFila).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<ClienteForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not ClienteListaFila fila)
        {
            MessageBox.Show("Seleccione un cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró el cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<ClienteForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not ClienteListaFila fila)
            return;
        if (MessageBox.Show($"¿Eliminar cliente {fila.Nombre} {fila.Apellido}?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
