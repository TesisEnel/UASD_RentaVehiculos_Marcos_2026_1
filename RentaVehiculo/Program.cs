using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo.Data.Context;
using RentaVehiculo.UI.Clientes;
using RentaVehiculo.UI.Facturas;
using RentaVehiculo.UI.Login;
using RentaVehiculo.UI.Mantenimientos;
using RentaVehiculo.UI.Rentas;
using RentaVehiculo.UI.Reservas;
using RentaVehiculo.UI.Usuarios;
using RentaVehiculo.UI.Vehiculos;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo;

static class Program
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        while (true)
        {
            using (var login = ServiceProvider.GetRequiredService<LoginForm>())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;
            }

            var main = ServiceProvider.GetRequiredService<MainForm>();
            Application.Run(main);
            if (!main.RequiereNuevoLogin)
                return;
        }
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["RentaVehiculos"]?.ConnectionString
            ?? throw new InvalidOperationException(
                "Defina la cadena de conexión 'RentaVehiculos' en App.config (connectionStrings).");

        services.AddDbContext<RentaVehiculosContext>(
            options => options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure();
            }),
            ServiceLifetime.Transient,
            ServiceLifetime.Transient);

        services.AddTransient<LoginForm>();
        services.AddTransient<MainForm>();

        services.AddTransient<VehiculoList>();
        services.AddTransient<VehiculoForm>();
        services.AddTransient<ClienteList>();
        services.AddTransient<ClienteForm>();
        services.AddTransient<ReservaList>();
        services.AddTransient<ReservaForm>();
        services.AddTransient<RentaList>();
        services.AddTransient<RentaForm>();
        services.AddTransient<MantenimientoList>();
        services.AddTransient<MantenimientoForm>();
        services.AddTransient<FacturaList>();
        services.AddTransient<FacturaForm>();
        services.AddTransient<UsuarioList>();
        services.AddTransient<UsuarioForm>();

        services.AddTransient<VehiculoService>();
        services.AddTransient<ClienteService>();
        services.AddTransient<ReservaService>();
        services.AddTransient<RentaService>();
        services.AddTransient<MantenimientoService>();
        services.AddTransient<FacturaService>();
        services.AddTransient<UsuarioService>();
        services.AddTransient<DashboardService>();
        services.AddTransient<SeleccionCatalogoService>();
    }
}
