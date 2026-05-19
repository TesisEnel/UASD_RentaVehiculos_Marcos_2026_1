using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Usuarios;

public partial class UsuarioList : Form
{
    private readonly UsuarioService _service;

    public UsuarioList(UsuarioService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void UsuarioList_Load(object sender, EventArgs e)
    {
        if (!SesionActual.EsAdministrador)
        {
            MessageBox.Show("Solo un administrador puede gestionar usuarios.", "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = nameof(UsuarioListaFila.Id), MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = nameof(UsuarioListaFila.Nombre), MinimumWidth = 100, FillWeight = 85 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Apellido", DataPropertyName = nameof(UsuarioListaFila.Apellido), MinimumWidth = 100, FillWeight = 85 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Usuario", DataPropertyName = nameof(UsuarioListaFila.NombreUsuario), MinimumWidth = 100, FillWeight = 85 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", DataPropertyName = nameof(UsuarioListaFila.Email), MinimumWidth = 160, FillWeight = 120 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Rol", DataPropertyName = nameof(UsuarioListaFila.RolTexto), MinimumWidth = 110, FillWeight = 75 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Activo", DataPropertyName = nameof(UsuarioListaFila.ActivoTexto), MinimumWidth = 72, FillWeight = 50 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private static UsuarioListaFila MapearFila(Usuario u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        Apellido = u.Apellido,
        NombreUsuario = u.NombreUsuario,
        Email = u.Email,
        RolTexto = UsuarioRoles.Normalizar(u.Rol),
        ActivoTexto = u.Activo ? "Sí" : "No"
    };

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetList(u => true);
            dataGridView1.DataSource = list.Select(MapearFila).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<UsuarioForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private async void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not UsuarioListaFila fila)
        {
            MessageBox.Show("Seleccione un usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var entidad = await _service.Buscar(fila.Id);
        if (entidad is null)
        {
            MessageBox.Show("No se encontró el usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<UsuarioForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not UsuarioListaFila fila)
            return;
        if (MessageBox.Show($"¿Eliminar usuario {fila.NombreUsuario}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
