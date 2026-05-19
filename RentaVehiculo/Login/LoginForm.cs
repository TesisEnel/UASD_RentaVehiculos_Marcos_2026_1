using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Login;

public partial class LoginForm : Form
{
    private readonly UsuarioService _usuarios;

    public LoginForm(UsuarioService usuarios)
    {
        InitializeComponent();
        _usuarios = usuarios;
    }

    private async void btnIngresar_Click(object? sender, EventArgs e)
    {
        btnIngresar.Enabled = false;
        try
        {
            var user = await _usuarios.ValidarInicioSesionAsync(txtUsuario.Text, txtPassword.Text);
            if (user is null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos, o cuenta inactiva.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.SelectAll();
                txtPassword.Focus();
                return;
            }

            SesionActual.Iniciar(user);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo iniciar sesión: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnIngresar.Enabled = true;
        }
    }
}
