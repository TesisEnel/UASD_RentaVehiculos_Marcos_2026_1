using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Vehiculos;

public partial class VehiculoForm : Form
{
    private readonly VehiculoService _service;
    private readonly SeleccionCatalogoService _catalogos;
    private Vehiculo? _entidad;
    private bool _guardando;

    public VehiculoForm(VehiculoService service, SeleccionCatalogoService catalogos) : this(service, catalogos, null)
    {
    }

    public VehiculoForm(VehiculoService service, SeleccionCatalogoService catalogos, Vehiculo? entidad)
    {
        InitializeComponent();
        _service = service;
        _catalogos = catalogos;
        _entidad = entidad;
        if (_entidad != null)
            Cargar(_entidad);
        else
            chkActivo.Checked = true;

        Load += VehiculoForm_Load;
    }

    private async void VehiculoForm_Load(object? sender, EventArgs e)
    {
        Load -= VehiculoForm_Load;
        try
        {
            var sucursales = await _catalogos.ObtenerSucursalesActivasAsync();
            var items = new List<ItemListaId> { new() { Id = 0, Nombre = "(Ninguna)" } };
            items.AddRange(sucursales);
            ComboSeleccion.Enlazar(cboSucursal, items, _entidad?.IdSucursal ?? 0);
            var estados = VehiculoEstadosUi.ListaParaSeleccion(_entidad?.Estado);
            ComboSeleccion.Enlazar(cboEstado, estados, _entidad?.Estado > 0 ? _entidad.Estado : null);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudieron cargar las sucursales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Cargar(Vehiculo v)
    {
        txtMarca.Text = v.Marca;
        txtModelo.Text = v.Modelo;
        numAño.Value = Math.Clamp(v.Año, (int)numAño.Minimum, (int)numAño.Maximum);
        txtPlaca.Text = v.Placa;
        numKm.Value = Math.Clamp(v.Kilometraje, 0, (int)numKm.Maximum);
        numPrecio.Value = Math.Clamp(v.PrecioPorDia, 0, numPrecio.Maximum);
        chkActivo.Checked = v.Activo;
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (_guardando)
            return;

        if (string.IsNullOrWhiteSpace(txtMarca.Text)
            || string.IsNullOrWhiteSpace(txtModelo.Text)
            || string.IsNullOrWhiteSpace(txtPlaca.Text))
        {
            MessageBox.Show("Marca, modelo y placa son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (numPrecio.Value <= 0)
        {
            MessageBox.Show("Indique un precio por día mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idEstado = ComboSeleccion.IdSeleccionado(cboEstado);
        if (idEstado <= 0)
        {
            MessageBox.Show("Seleccione el estado del vehículo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var esNuevo = _entidad == null || _entidad.Id == 0;
        _entidad ??= new Vehiculo();

        _entidad.Marca = txtMarca.Text.Trim();
        _entidad.Modelo = txtModelo.Text.Trim();
        _entidad.Año = (int)numAño.Value;
        _entidad.Placa = txtPlaca.Text.Trim();
        _entidad.Kilometraje = (int)numKm.Value;
        _entidad.PrecioPorDia = numPrecio.Value;
        _entidad.Estado = idEstado;
        _entidad.Activo = chkActivo.Checked;
        var idSuc = ComboSeleccion.IdSeleccionado(cboSucursal);
        _entidad.IdSucursal = idSuc == 0 ? null : idSuc;

        if (_entidad.TipoCombustible == 0 && esNuevo)
            _entidad.TipoCombustible = 1;
        if (_entidad.TipoTransmision == 0 && esNuevo)
            _entidad.TipoTransmision = 1;
        if (_entidad.NumeroAsientos == 0 && esNuevo)
            _entidad.NumeroAsientos = 5;
        if (_entidad.CapacidadMaletero == 0 && esNuevo)
            _entidad.CapacidadMaletero = 300;
        if (_entidad.TipoVehiculo == 0 && esNuevo)
            _entidad.TipoVehiculo = 1;

        _guardando = true;
        try
        {
            btnGuardar.Enabled = false;
            if (await _service.Guardar(_entidad))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show("No se pudo guardar el vehículo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _guardando = false;
            btnGuardar.Enabled = true;
        }
    }
}
