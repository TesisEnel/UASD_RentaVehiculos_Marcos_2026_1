namespace RentaVehiculo.UI.Reservas
{
    partial class ReservaForm
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
            cboCliente = new ComboBox();
            cboVehiculo = new ComboBox();
            dtpInicio = new DateTimePicker();
            dtpFin = new DateTimePicker();
            numMonto = new NumericUpDown();
            cboEstado = new ComboBox();
            chkDepPagado = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCliente.Location = new Point(200, 22);
            cboCliente.Width = 300;
            cboCliente.TabIndex = 0;
            cboVehiculo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVehiculo.Location = new Point(200, 62);
            cboVehiculo.Width = 300;
            cboVehiculo.TabIndex = 1;
            dtpInicio.Location = new Point(200, 102);
            dtpFin.Location = new Point(200, 142);
            numMonto.DecimalPlaces = 2;
            numMonto.Location = new Point(200, 182);
            numMonto.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Location = new Point(200, 222);
            cboEstado.Width = 300;
            chkDepPagado.Location = new Point(200, 262);
            chkDepPagado.Text = "Depósito pagado";
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
                new Label { Text = "Cliente:", Location = new Point(20, 25), AutoSize = true },
                new Label { Text = "Vehículo:", Location = new Point(20, 65), AutoSize = true },
                new Label { Text = "Inicio reserva:", Location = new Point(20, 105), AutoSize = true },
                new Label { Text = "Fin reserva:", Location = new Point(20, 145), AutoSize = true },
                new Label { Text = "Monto depósito:", Location = new Point(20, 185), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 225), AutoSize = true },
                cboCliente, cboVehiculo, dtpInicio, dtpFin, numMonto, cboEstado, chkDepPagado, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(540, 390);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reserva";
            ResumeLayout(false);
        }

        private ComboBox cboCliente;
        private ComboBox cboVehiculo;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private NumericUpDown numMonto;
        private ComboBox cboEstado;
        private CheckBox chkDepPagado;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
