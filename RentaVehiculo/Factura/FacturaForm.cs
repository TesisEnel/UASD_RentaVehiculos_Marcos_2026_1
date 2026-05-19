using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Facturas;

public partial class FacturaForm : Form
{
    private readonly FacturaService _service;
    private readonly SeleccionCatalogoService _catalogos;
    private Factura? _entidad;

    public FacturaForm(FacturaService service, SeleccionCatalogoService catalogos) : this(service, catalogos, null)
    {
    }

    public FacturaForm(FacturaService service, SeleccionCatalogoService catalogos, Factura? entidad)
    {
        InitializeComponent();
        _service = service;
        _catalogos = catalogos;
        _entidad = entidad;
        if (_entidad != null)
        {
            txtNumero.Text = _entidad.NumeroFactura;
            numSub.Value = _entidad.Subtotal;
            numImp.Value = _entidad.Impuestos;
            numTot.Value = _entidad.Total;
            numMetodo.Value = _entidad.MetodoPago;
            numEstado.Value = _entidad.Estado;
        }
        else
        {
            txtNumero.Text = $"FAC-{Guid.NewGuid().ToString("N")[..10].ToUpperInvariant()}";
            numSub.Value = 100;
            numImp.Value = 15;
            numTot.Value = 115;
            numMetodo.Value = 1;
            numEstado.Value = 1;
        }

        Load += FacturaForm_Load;
    }

    private async void FacturaForm_Load(object? sender, EventArgs e)
    {
        Load -= FacturaForm_Load;
        try
        {
            var rentas = await _catalogos.ObtenerRentasResumenAsync();
            ComboSeleccion.Enlazar(cboRenta, rentas, _entidad?.IdRenta);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudieron cargar las rentas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNumero.Text))
        {
            MessageBox.Show("Número de factura obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idRenta = ComboSeleccion.IdSeleccionado(cboRenta);
        if (idRenta <= 0)
        {
            MessageBox.Show("Seleccione la renta asociada.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Factura();
        _entidad.IdRenta = idRenta;
        _entidad.NumeroFactura = txtNumero.Text.Trim();
        _entidad.Subtotal = numSub.Value;
        _entidad.Impuestos = numImp.Value;
        _entidad.Total = numTot.Value;
        _entidad.MetodoPago = (int)numMetodo.Value;
        _entidad.Estado = (int)numEstado.Value;
        if (_entidad.FechaEmision == default)
            _entidad.FechaEmision = DateTime.Now;

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
