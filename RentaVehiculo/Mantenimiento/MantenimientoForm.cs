using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Mantenimientos;

public partial class MantenimientoForm : Form
{
    private readonly MantenimientoService _service;
    private readonly SeleccionCatalogoService _catalogos;
    private Mantenimiento? _entidad;

    public MantenimientoForm(MantenimientoService service, SeleccionCatalogoService catalogos) : this(service, catalogos, null)
    {
    }

    public MantenimientoForm(MantenimientoService service, SeleccionCatalogoService catalogos, Mantenimiento? entidad)
    {
        InitializeComponent();
        _service = service;
        _catalogos = catalogos;
        _entidad = entidad;
        if (_entidad != null)
        {
            numCosto.Value = _entidad.Costo;
            dtpInicio.Value = _entidad.FechaInicio;
            if (_entidad.FechaFin.HasValue)
                dtpFin.Value = _entidad.FechaFin.Value;
            numKm.Value = _entidad.KilometrajeMantenimiento;
            numProx.Value = _entidad.ProximoMantenimiento;
        }

        Load += MantenimientoForm_Load;
    }

    private async void MantenimientoForm_Load(object? sender, EventArgs e)
    {
        Load -= MantenimientoForm_Load;
        try
        {
            var vehiculos = await _catalogos.ObtenerVehiculosAsync(soloActivos: false);
            var proveedores = await _catalogos.ObtenerProveedoresMantenimientoAsync();

            ComboSeleccion.Enlazar(cboVehiculo, vehiculos, _entidad?.IdVehiculo);
            ComboSeleccion.Enlazar(cboTipo, MantenimientoTiposUi.ListaParaSeleccion(_entidad?.TipoMantenimiento),
                _entidad?.TipoMantenimiento > 0 ? _entidad.TipoMantenimiento : null);
            ComboSeleccion.Enlazar(cboEstado, MantenimientoEstadosUi.ListaParaSeleccion(_entidad?.Estado),
                _entidad?.Estado > 0 ? _entidad.Estado : null);
            ComboSeleccion.CargarProveedoresSugeridos(cboProveedor, proveedores, _entidad?.Proveedor);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudieron cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        var idVeh = ComboSeleccion.IdSeleccionado(cboVehiculo);
        if (idVeh <= 0)
        {
            MessageBox.Show("Seleccione un vehículo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tipo = ComboSeleccion.IdSeleccionado(cboTipo);
        var estado = ComboSeleccion.IdSeleccionado(cboEstado);
        if (tipo <= 0 || estado <= 0)
        {
            MessageBox.Show("Seleccione tipo y estado del mantenimiento.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Mantenimiento();
        _entidad.IdVehiculo = idVeh;
        _entidad.TipoMantenimiento = tipo;
        _entidad.Costo = numCosto.Value;
        _entidad.FechaInicio = dtpInicio.Value;
        _entidad.FechaFin = dtpFin.Value;
        _entidad.KilometrajeMantenimiento = (int)numKm.Value;
        _entidad.ProximoMantenimiento = (int)numProx.Value;
        _entidad.Estado = estado;
        _entidad.Proveedor = string.IsNullOrWhiteSpace(cboProveedor.Text) ? null : cboProveedor.Text.Trim();

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
