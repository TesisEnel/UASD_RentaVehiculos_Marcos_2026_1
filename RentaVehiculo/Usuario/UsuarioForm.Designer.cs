namespace RentaVehiculo.UI.Usuarios
{
    partial class UsuarioForm
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
            txtUsuario = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            cboRol = new ComboBox();
            cboSucursal = new ComboBox();
            chkActivo = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            txtNombre.Location = new Point(200, 24);
            txtNombre.Size = new Size(260, 23);
            txtApellido.Location = new Point(200, 64);
            txtApellido.Size = new Size(260, 23);
            txtUsuario.Location = new Point(200, 104);
            txtUsuario.Size = new Size(260, 23);
            txtEmail.Location = new Point(200, 144);
            txtEmail.Size = new Size(260, 23);
            txtPassword.Location = new Point(200, 184);
            txtPassword.Size = new Size(260, 23);
            txtPassword.PasswordChar = '•';
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Location = new Point(200, 224);
            cboRol.Size = new Size(260, 23);
            cboSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSucursal.Location = new Point(200, 262);
            cboSucursal.Size = new Size(260, 23);
            chkActivo.Location = new Point(200, 304);
            chkActivo.Text = "Activo";
            btnGuardar.Location = new Point(200, 356);
            btnGuardar.Size = new Size(115, 38);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Location = new Point(325, 356);
            btnCancelar.Size = new Size(115, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.DialogResult = DialogResult.Cancel;
            Controls.AddRange(new Control[] {
                new Label { Text = "Nombre:", Location = new Point(20, 27), AutoSize = true },
                new Label { Text = "Apellido:", Location = new Point(20, 67), AutoSize = true },
                new Label { Text = "Nombre usuario:", Location = new Point(20, 107), AutoSize = true },
                new Label { Text = "Email:", Location = new Point(20, 147), AutoSize = true },
                new Label { Text = "Contraseña:", Location = new Point(20, 187), AutoSize = true },
                new Label { Text = "Rol:", Location = new Point(20, 227), AutoSize = true },
                new Label { Text = "Sucursal (opc.):", Location = new Point(20, 267), AutoSize = true },
                txtNombre, txtApellido, txtUsuario, txtEmail, txtPassword, cboRol, cboSucursal, chkActivo, btnGuardar, btnCancelar });
            AutoScroll = true;
            ClientSize = new Size(500, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuario";
            ResumeLayout(false);
        }

        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtUsuario;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cboRol;
        private ComboBox cboSucursal;
        private CheckBox chkActivo;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
