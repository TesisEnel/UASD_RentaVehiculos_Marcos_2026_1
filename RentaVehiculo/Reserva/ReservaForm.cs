using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Reservas;

public partial class ReservaForm : Form
{
    private readonly ReservaService _service;
    private readonly SeleccionCatalogoService _catalogos;
    private Reserva? _entidad;

    public ReservaForm(ReservaService service, SeleccionCatalogoService catalogos) : this(service, catalogos, null)
    {
    }

    public ReservaForm(ReservaService service, SeleccionCatalogoService catalogos, Reserva? entidad)
    {
        InitializeComponent();
        _service = service;
        _catalogos = catalogos;
        _entidad = entidad;
        if (_entidad != null)
        {
            dtpInicio.Value = _entidad.FechaInicioReserva;
            dtpFin.Value = _entidad.FechaFinReserva;
            numMonto.Value = _entidad.MontoDeposito;
            chkDepPagado.Checked = _entidad.DepositoPagado;
        }
        else
        {
            dtpInicio.Value = DateTime.Now.Date;
            dtpFin.Value = DateTime.Now.Date.AddDays(1);
            numMonto.Value = 50;
        }

        Load += ReservaForm_Load;
    }

    private async void ReservaForm_Load(object? sender, EventArgs e)
    {
        Load -= ReservaForm_Load;
        try
        {
            var clientes = await _catalogos.ObtenerClientesAsync();
            var vehiculos = await _catalogos.ObtenerVehiculosAsync();
            ComboSeleccion.Enlazar(cboCliente, clientes, _entidad?.IdCliente);
            ComboSeleccion.Enlazar(cboVehiculo, vehiculos, _entidad?.IdVehiculo);
            var estados = ReservaEstadosUi.ListaParaSeleccion(_entidad?.Estado);
            ComboSeleccion.Enlazar(cboEstado, estados, _entidad?.Estado > 0 ? _entidad.Estado : null);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudieron cargar los catálogos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        var idCliente = ComboSeleccion.IdSeleccionado(cboCliente);
        var idVehiculo = ComboSeleccion.IdSeleccionado(cboVehiculo);
        if (idCliente <= 0 || idVehiculo <= 0)
        {
            MessageBox.Show("Seleccione cliente y vehículo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idEstado = ComboSeleccion.IdSeleccionado(cboEstado);
        if (idEstado <= 0)
        {
            MessageBox.Show("Seleccione el estado de la reserva.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Reserva();
        _entidad.IdCliente = idCliente;
        _entidad.IdVehiculo = idVehiculo;
        _entidad.FechaInicioReserva = dtpInicio.Value;
        _entidad.FechaFinReserva = dtpFin.Value;
        _entidad.MontoDeposito = numMonto.Value;
        _entidad.Estado = idEstado;
        _entidad.DepositoPagado = chkDepPagado.Checked;
        if (_entidad.FechaReserva == default)
            _entidad.FechaReserva = DateTime.Now;

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
