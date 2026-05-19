namespace RentaVehiculo.UI.Clientes
{
    partial class ClienteForm
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
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtEmail = new TextBox();
            txtTelefono = new TextBox();
            txtLicencia = new TextBox();
            dtpVencLic = new DateTimePicker();
            dtpNac = new DateTimePicker();
            cboTipoCliente = new ComboBox();
            chkActivo = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            txtNombre.Location = new Point(180, 24);
            txtNombre.Size = new Size(280, 23);
            txtApellido.Location = new Point(180, 64);
            txtApellido.Size = new Size(280, 23);
            txtEmail.Location = new Point(180, 104);
            txtEmail.Size = new Size(280, 23);
            txtTelefono.Location = new Point(180, 144);
            txtTelefono.Size = new Size(200, 23);
            txtLicencia.Location = new Point(180, 184);
            txtLicencia.Size = new Size(200, 23);
            dtpVencLic.Location = new Point(180, 224);
            dtpNac.Location = new Point(180, 264);
            cboTipoCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoCliente.Location = new Point(180, 302);
            cboTipoCliente.Size = new Size(280, 23);
            chkActivo.Location = new Point(180, 344);
            chkActivo.Text = "Activo";
            chkActivo.AutoSize = true;
            btnGuardar.Location = new Point(180, 396);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Location = new Point(305, 396);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.DialogResult = DialogResult.Cancel;
            Controls.AddRange(new Control[] { new Label { Text = "Nombre:", Location = new Point(20, 27), AutoSize = true },
                new Label { Text = "Apellido:", Location = new Point(20, 67), AutoSize = true },
                new Label { Text = "Email:", Location = new Point(20, 107), AutoSize = true },
                new Label { Text = "Teléfono:", Location = new Point(20, 147), AutoSize = true },
                new Label { Text = "Licencia:", Location = new Point(20, 187), AutoSize = true },
                new Label { Text = "Venc. licencia:", Location = new Point(20, 227), AutoSize = true },
                new Label { Text = "Nacimiento:", Location = new Point(20, 267), AutoSize = true },
                new Label { Text = "Tipo cliente:", Location = new Point(20, 307), AutoSize = true },
                txtNombre, txtApellido, txtEmail, txtTelefono, txtLicencia, dtpVencLic, dtpNac, cboTipoCliente, chkActivo, btnGuardar, btnCancelar });
            AutoScroll = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize = new Size(500, 460);
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cliente";
            ResumeLayout(false);
        }

        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private TextBox txtLicencia;
        private DateTimePicker dtpVencLic;
        private DateTimePicker dtpNac;
        private ComboBox cboTipoCliente;
        private CheckBox chkActivo;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
