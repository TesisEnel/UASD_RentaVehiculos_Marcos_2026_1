namespace RentaVehiculo.UI.Rentas
{
    partial class RentaForm
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
            cboEmpleado = new ComboBox();
            cboSucRec = new ComboBox();
            cboSucEnt = new ComboBox();
            dtpInicio = new DateTimePicker();
            dtpFin = new DateTimePicker();
            numKmIni = new NumericUpDown();
            numCostoDia = new NumericUpDown();
            numDias = new NumericUpDown();
            numCostoTot = new NumericUpDown();
            numDep = new NumericUpDown();
            cboEstado = new ComboBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            foreach (var cb in new[] { cboCliente, cboVehiculo, cboEmpleado, cboSucRec, cboSucEnt })
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCliente.Location = new Point(200, 22);
            cboCliente.Width = 300;
            cboVehiculo.Location = new Point(200, 62);
            cboVehiculo.Width = 300;
            cboEmpleado.Location = new Point(200, 102);
            cboEmpleado.Width = 300;
            cboSucRec.Location = new Point(200, 142);
            cboSucRec.Width = 300;
            cboSucEnt.Location = new Point(200, 182);
            cboSucEnt.Width = 300;
            dtpInicio.Location = new Point(200, 222);
            dtpFin.Location = new Point(200, 262);
            numKmIni.Location = new Point(200, 302);
            numKmIni.Maximum = 10000000;
            numCostoDia.Location = new Point(200, 342);
            numCostoDia.DecimalPlaces = 2;
            numCostoDia.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numDias.Location = new Point(200, 382);
            numDias.Maximum = 1000;
            numDias.Minimum = 1;
            numCostoTot.Location = new Point(200, 422);
            numCostoTot.DecimalPlaces = 2;
            numCostoTot.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numDep.Location = new Point(200, 462);
            numDep.DecimalPlaces = 2;
            numDep.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Location = new Point(200, 502);
            cboEstado.Width = 300;
            btnGuardar.Location = new Point(200, 554);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(325, 554);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {
                new Label { Text = "Cliente:", Location = new Point(20, 25), AutoSize = true },
                new Label { Text = "Vehículo:", Location = new Point(20, 65), AutoSize = true },
                new Label { Text = "Empleado (opc.):", Location = new Point(20, 105), AutoSize = true },
                new Label { Text = "Sucursal recogida:", Location = new Point(20, 145), AutoSize = true },
                new Label { Text = "Sucursal entrega (opc.):", Location = new Point(20, 185), AutoSize = true },
                new Label { Text = "Inicio:", Location = new Point(20, 225), AutoSize = true },
                new Label { Text = "Fin programada:", Location = new Point(20, 265), AutoSize = true },
                new Label { Text = "Km inicial:", Location = new Point(20, 305), AutoSize = true },
                new Label { Text = "Costo/día:", Location = new Point(20, 345), AutoSize = true },
                new Label { Text = "Días:", Location = new Point(20, 385), AutoSize = true },
                new Label { Text = "Costo total:", Location = new Point(20, 425), AutoSize = true },
                new Label { Text = "Depósito:", Location = new Point(20, 465), AutoSize = true },
                new Label { Text = "Estado:", Location = new Point(20, 505), AutoSize = true },
                cboCliente, cboVehiculo, cboEmpleado, cboSucRec, cboSucEnt,
                dtpInicio, dtpFin, numKmIni, numCostoDia, numDias, numCostoTot, numDep, cboEstado,
                btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(540, 620);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Renta";
            ResumeLayout(false);
        }

        private ComboBox cboCliente;
        private ComboBox cboVehiculo;
        private ComboBox cboEmpleado;
        private ComboBox cboSucRec;
        private ComboBox cboSucEnt;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private NumericUpDown numKmIni;
        private NumericUpDown numCostoDia;
        private NumericUpDown numDias;
        private NumericUpDown numCostoTot;
        private NumericUpDown numDep;
        private ComboBox cboEstado;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
