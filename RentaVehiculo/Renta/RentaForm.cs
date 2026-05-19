using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Rentas;

public partial class RentaForm : Form
{
    private readonly RentaService _service;
    private readonly SeleccionCatalogoService _catalogos;
    private Renta? _entidad;
    private bool _suprimirRecalculoTotal;

    public RentaForm(RentaService service, SeleccionCatalogoService catalogos) : this(service, catalogos, null)
    {
    }

    public RentaForm(RentaService service, SeleccionCatalogoService catalogos, Renta? entidad)
    {
        InitializeComponent();
        _service = service;
        _catalogos = catalogos;
        _entidad = entidad;

        numCostoDia.ValueChanged += (_, _) => ActualizarCostoTotalDesdeDiasYDiario();
        numDias.ValueChanged += (_, _) => ActualizarCostoTotalDesdeDiasYDiario();

        _suprimirRecalculoTotal = true;
        if (_entidad != null)
        {
            dtpInicio.Value = _entidad.FechaInicio;
            dtpFin.Value = _entidad.FechaFinProgramada;
            numKmIni.Value = _entidad.KilometrajeInicial;
            numCostoDia.Value = _entidad.CostoDiario;
            numDias.Value = Math.Max(1, _entidad.DiasRentados);
            numCostoTot.Value = _entidad.CostoTotal;
            numDep.Value = _entidad.Deposito;
        }
        else
        {
            dtpInicio.Value = DateTime.Now;
            dtpFin.Value = DateTime.Now.AddDays(1);
            numCostoDia.Value = 30;
            numDias.Value = 1;
            numCostoTot.Value = 30;
            numDep.Value = 100;
        }

        _suprimirRecalculoTotal = false;

        Load += RentaForm_Load;
    }

    private void ActualizarCostoTotalDesdeDiasYDiario()
    {
        if (_suprimirRecalculoTotal)
            return;
        var dias = Math.Max(1, (int)numDias.Value);
        var total = Math.Round(numCostoDia.Value * dias, 2, MidpointRounding.AwayFromZero);
        var max = numCostoTot.Maximum;
        _suprimirRecalculoTotal = true;
        try
        {
            numCostoTot.Value = total > max ? max : total;
        }
        finally
        {
            _suprimirRecalculoTotal = false;
        }
    }

    private async void RentaForm_Load(object? sender, EventArgs e)
    {
        Load -= RentaForm_Load;
        try
        {
            var clientes = await _catalogos.ObtenerClientesAsync();
            var vehiculos = await _catalogos.ObtenerVehiculosAsync();
            var usuarios = await _catalogos.ObtenerUsuariosActivosAsync();
            var sucursales = await _catalogos.ObtenerSucursalesActivasAsync();

            var empleados = new List<ItemListaId> { new() { Id = 0, Nombre = "(Sin empleado)" } };
            empleados.AddRange(usuarios);

            var sucEnt = new List<ItemListaId> { new() { Id = 0, Nombre = "(Sin sucursal de entrega)" } };
            sucEnt.AddRange(sucursales);

            ComboSeleccion.Enlazar(cboCliente, clientes, _entidad?.IdCliente);
            ComboSeleccion.Enlazar(cboVehiculo, vehiculos, _entidad?.IdVehiculo);
            ComboSeleccion.Enlazar(cboEmpleado, empleados, _entidad?.IdEmpleado ?? 0);
            ComboSeleccion.Enlazar(cboSucRec, sucursales, _entidad?.SucursalRecogida);
            ComboSeleccion.Enlazar(cboSucEnt, sucEnt, _entidad?.SucursalEntrega ?? 0);

            var estados = RentaEstadosUi.ListaParaSeleccion(_entidad?.Estado);
            ComboSeleccion.Enlazar(cboEstado, estados, _entidad?.Estado ?? 1);
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
        var idSucRec = ComboSeleccion.IdSeleccionado(cboSucRec);
        if (idCliente <= 0 || idVehiculo <= 0)
        {
            MessageBox.Show("Seleccione cliente y vehículo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (idSucRec <= 0)
        {
            MessageBox.Show("Seleccione sucursal de recogida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idEmp = ComboSeleccion.IdSeleccionado(cboEmpleado);
        var idSucEnt = ComboSeleccion.IdSeleccionado(cboSucEnt);
        var estado = ComboSeleccion.IdSeleccionado(cboEstado);
        if (estado <= 0)
        {
            MessageBox.Show("Seleccione el estado de la renta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Renta();
        _entidad.IdCliente = idCliente;
        _entidad.IdVehiculo = idVehiculo;
        _entidad.IdEmpleado = idEmp == 0 ? null : idEmp;
        _entidad.SucursalRecogida = idSucRec;
        _entidad.SucursalEntrega = idSucEnt == 0 ? null : idSucEnt;
        _entidad.FechaInicio = dtpInicio.Value;
        _entidad.FechaFinProgramada = dtpFin.Value;
        _entidad.KilometrajeInicial = (int)numKmIni.Value;
        _entidad.CostoDiario = numCostoDia.Value;
        _entidad.DiasRentados = (int)numDias.Value;
        _entidad.CostoTotal = numCostoTot.Value;
        _entidad.Deposito = numDep.Value;
        _entidad.Estado = estado;
        if (_entidad.FechaCreacion == default)
            _entidad.FechaCreacion = DateTime.Now;
        if (_entidad.Descuento == 0 && _entidad.CostoAdicionales == 0)
        {
            _entidad.Descuento = 0;
            _entidad.CostoAdicionales = 0;
        }

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
