namespace RentaVehiculo.UI.Facturas
{
    partial class FacturaForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cboRenta = new ComboBox();
            txtNumero = new TextBox();
            numSub = new NumericUpDown();
            numImp = new NumericUpDown();
            numTot = new NumericUpDown();
            numMetodo = new NumericUpDown();
            numEstado = new NumericUpDown();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            cboRenta.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRenta.Location = new Point(200, 22);
            cboRenta.Width = 340;
            txtNumero.Location = new Point(200, 62);
            txtNumero.Width = 260;
            numSub.DecimalPlaces = numImp.DecimalPlaces = numTot.DecimalPlaces = 2;
            numSub.Location = new Point(200, 102);
            numImp.Location = new Point(200, 142);
            numTot.Location = new Point(200, 182);
            numSub.Maximum = numImp.Maximum = numTot.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numMetodo.Location = new Point(200, 222);
            numEstado.Location = new Point(200, 262);
            btnGuardar.Location = new Point(200, 314);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(325, 314);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {
                new Label { Text = "Renta:", Location = new Point(20, 25), AutoSize = true },
                new Label { Text = "Número factura:", Location = new Point(20, 65), AutoSize = true },
                new Label { Text = "Subtotal:", Location = new Point(20, 105), AutoSize = true },
                new Label { Text = "Impuestos:", Location = new Point(20, 145), AutoSize = true },
                new Label { Text = "Total:", Location = new Point(20, 185), AutoSize = true },
                new Label { Text = "Método pago:", Location = new Point(20, 225), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 265), AutoSize = true },
                cboRenta, txtNumero, numSub, numImp, numTot, numMetodo, numEstado, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(580, 390);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Factura";
            ResumeLayout(false);
        }

        private ComboBox cboRenta;
        private TextBox txtNumero;
        private NumericUpDown numSub;
        private NumericUpDown numImp;
        private NumericUpDown numTot;
        private NumericUpDown numMetodo;
        private NumericUpDown numEstado;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
