namespace RentaVehiculo.UI.Mantenimientos
{
    partial class MantenimientoForm
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
            cboVehiculo = new ComboBox();
            cboTipo = new ComboBox();
            numCosto = new NumericUpDown();
            dtpInicio = new DateTimePicker();
            dtpFin = new DateTimePicker();
            numKm = new NumericUpDown();
            numProx = new NumericUpDown();
            cboEstado = new ComboBox();
            cboProveedor = new ComboBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            cboVehiculo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVehiculo.Location = new Point(180, 22);
            cboVehiculo.Width = 300;
            cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipo.Location = new Point(180, 62);
            cboTipo.Width = 300;
            numCosto.DecimalPlaces = 2;
            numCosto.Location = new Point(180, 102);
            numCosto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            dtpInicio.Location = new Point(180, 142);
            dtpFin.Location = new Point(180, 182);
            numKm.Location = new Point(180, 222);
            numKm.Maximum = 10000000;
            numProx.Location = new Point(180, 262);
            numProx.Maximum = 10000000;
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Location = new Point(180, 302);
            cboEstado.Width = 300;
            cboProveedor.Location = new Point(180, 342);
            cboProveedor.Width = 300;
            btnGuardar.Location = new Point(180, 394);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(305, 394);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {
                new Label { Text = "Vehículo:", Location = new Point(20, 25), AutoSize = true },
                new Label { Text = "Tipo:", Location = new Point(20, 65), AutoSize = true },
                new Label { Text = "Costo:", Location = new Point(20, 105), AutoSize = true },
                new Label { Text = "Inicio:", Location = new Point(20, 145), AutoSize = true },
                new Label { Text = "Fin:", Location = new Point(20, 185), AutoSize = true },
                new Label { Text = "Km mant.:", Location = new Point(20, 225), AutoSize = true },
                new Label { Text = "Próx. km:", Location = new Point(20, 265), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 305), AutoSize = true },
                new Label { Text = "Proveedor:", Location = new Point(20, 345), AutoSize = true },
                cboVehiculo, cboTipo, numCosto, dtpInicio, dtpFin, numKm, numProx, cboEstado, cboProveedor, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(520, 460);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mantenimiento";
            ResumeLayout(false);
        }

        private ComboBox cboVehiculo;
        private ComboBox cboTipo;
        private NumericUpDown numCosto;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private NumericUpDown numKm;
        private NumericUpDown numProx;
        private ComboBox cboEstado;
        private ComboBox cboProveedor;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
