using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Usuarios;

public partial class UsuarioForm : Form
{
    private readonly UsuarioService _service;
    private readonly SeleccionCatalogoService _catalogos;
    private Usuario? _entidad;

    public UsuarioForm(UsuarioService service, SeleccionCatalogoService catalogos) : this(service, catalogos, null)
    {
    }

    public UsuarioForm(UsuarioService service, SeleccionCatalogoService catalogos, Usuario? entidad)
    {
        InitializeComponent();
        _service = service;
        _catalogos = catalogos;
        _entidad = entidad;

        UsuarioRolesUi.EnlazarRoles(cboRol, _entidad?.Rol);

        if (_entidad != null)
        {
            txtNombre.Text = _entidad.Nombre;
            txtApellido.Text = _entidad.Apellido;
            txtUsuario.Text = _entidad.NombreUsuario;
            txtEmail.Text = _entidad.Email;
            txtPassword.Text = "";
            txtPassword.PlaceholderText = "(sin cambiar)";
            chkActivo.Checked = _entidad.Activo;
        }
        else
        {
            txtPassword.PlaceholderText = "obligatoria en alta";
            chkActivo.Checked = true;
        }

        Load += UsuarioForm_Load;
    }

    private async void UsuarioForm_Load(object? sender, EventArgs e)
    {
        Load -= UsuarioForm_Load;
        try
        {
            var sucursales = await _catalogos.ObtenerSucursalesActivasAsync();
            var items = new List<ItemListaId> { new() { Id = 0, Nombre = "(Ninguna)" } };
            items.AddRange(sucursales);
            ComboSeleccion.Enlazar(cboSucursal, items, _entidad?.IdSucursal ?? 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudieron cargar las sucursales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            MessageBox.Show("Nombre y nombre de usuario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Usuario();
        _entidad.Nombre = txtNombre.Text.Trim();
        _entidad.Apellido = txtApellido.Text.Trim();
        _entidad.NombreUsuario = txtUsuario.Text.Trim();
        _entidad.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? $"{txtUsuario.Text.Trim()}@local" : txtEmail.Text.Trim();
        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            _entidad.PasswordHash = txtPassword.Text.Trim();
        else if (_entidad.Id == 0)
            _entidad.PasswordHash = "cambiar123";
        _entidad.Rol = UsuarioRolesUi.RolSeleccionado(cboRol);
        _entidad.Activo = chkActivo.Checked;
        var idSuc = ComboSeleccion.IdSeleccionado(cboSucursal);
        _entidad.IdSucursal = idSuc == 0 ? null : idSuc;

        try
        {
            if (await _service.Guardar(_entidad))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
