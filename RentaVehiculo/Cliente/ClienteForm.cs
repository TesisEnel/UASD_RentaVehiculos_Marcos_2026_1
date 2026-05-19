using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Clientes;

public partial class ClienteForm : Form
{
    private readonly ClienteService _service;
    private Cliente? _entidad;

    public ClienteForm(ClienteService service) : this(service, null)
    {
    }

    public ClienteForm(ClienteService service, Cliente? entidad)
    {
        InitializeComponent();
        _service = service;
        _entidad = entidad;

        var tipos = ClienteTiposUi.ListaParaSeleccion(_entidad?.TipoCliente);
        ComboSeleccion.Enlazar(cboTipoCliente, tipos, _entidad?.TipoCliente > 0 ? _entidad.TipoCliente : null);

        if (_entidad != null)
        {
            txtNombre.Text = _entidad.Nombre;
            txtApellido.Text = _entidad.Apellido;
            txtEmail.Text = _entidad.Email;
            txtTelefono.Text = _entidad.Telefono;
            txtLicencia.Text = _entidad.Licencia;
            dtpVencLic.Value = _entidad.FechaVencimientoLicencia;
            dtpNac.Value = _entidad.FechaNacimiento;
            chkActivo.Checked = _entidad.Activo;
        }
        else
            chkActivo.Checked = true;
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            MessageBox.Show("Nombre y email obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tipoCliente = ComboSeleccion.IdSeleccionado(cboTipoCliente);
        if (tipoCliente <= 0)
        {
            MessageBox.Show("Seleccione el tipo de cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Cliente();
        _entidad.Nombre = txtNombre.Text.Trim();
        _entidad.Apellido = txtApellido.Text.Trim();
        _entidad.Email = txtEmail.Text.Trim();
        _entidad.Telefono = txtTelefono.Text.Trim();
        _entidad.Licencia = string.IsNullOrWhiteSpace(txtLicencia.Text) ? "PED-" + Guid.NewGuid().ToString("N")[..8] : txtLicencia.Text.Trim();
        _entidad.FechaVencimientoLicencia = dtpVencLic.Value.Date;
        _entidad.FechaNacimiento = dtpNac.Value.Date;
        _entidad.TipoCliente = tipoCliente;
        _entidad.Activo = chkActivo.Checked;

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
