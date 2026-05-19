namespace RentaVehiculo.UI.Login
{
    partial class LoginForm
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
            lblTitulo = new Label();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnIngresar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Text = "RentCar Pro — Iniciar sesión";
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(24, 68);
            lblUsuario.Text = "Usuario:";
            txtUsuario.Location = new Point(24, 88);
            txtUsuario.Size = new Size(320, 23);
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(24, 128);
            lblPassword.Text = "Contraseña:";
            txtPassword.Location = new Point(24, 148);
            txtPassword.Size = new Size(320, 23);
            txtPassword.PasswordChar = '•';
            btnIngresar.Location = new Point(24, 200);
            btnIngresar.Size = new Size(150, 40);
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(194, 200);
            btnCancelar.Size = new Size(150, 40);
            btnCancelar.Text = "Salir";
            btnCancelar.UseVisualStyleBackColor = true;
            AcceptButton = btnIngresar;
            CancelButton = btnCancelar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 270);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio de sesión";
            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnIngresar);
            Controls.Add(btnCancelar);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Button btnCancelar;
    }
}
