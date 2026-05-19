using System.ComponentModel;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo.UI.Clientes;
using RentaVehiculo.UI.Facturas;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Mantenimientos;
using RentaVehiculo.UI.Rentas;
using RentaVehiculo.UI.Reservas;
using RentaVehiculo.UI.Services;
using RentaVehiculo.UI.Usuarios;
using RentaVehiculo.UI.Vehiculos;

namespace RentaVehiculo
{
    public partial class MainForm : Form
    {
        /// <summary>Índice del botón de navegación que muestra el panel principal (dashboard).</summary>
        private const int NavDashboard = -1;

        private readonly Color _sidebarBg = Color.FromArgb(15, 23, 42);
        private readonly Color _mainBg = Color.FromArgb(248, 250, 252);
        private readonly Color _accentBlue = Color.FromArgb(37, 99, 235);
        private readonly Color _textMuted = Color.FromArgb(148, 163, 184);
        private readonly Color _textDark = Color.FromArgb(15, 23, 42);

        private DashboardService? _dashboard;

        private Guna2Panel _pnlSidebar = null!;
        private Guna2Panel _pnlMain = null!;
        private readonly List<Guna2Button> _navButtons = new();
        private bool _abriendoModulo;
        private bool _navClickEnCurso;

        private Label? _kpiVehVal;
        private Label? _kpiVehSub;
        private Label? _kpiRentVal;
        private Label? _kpiRentSub;
        private Label? _kpiMantVal;
        private Label? _kpiMantSub;
        private FlowLayoutPanel? _flpAlerts;
        private Label? _lblProfileUser;
        private Label? _lblProfileMail;
        private Label? _lblProfileAvatar;

        /// <summary>True cuando el usuario cerró sesión y debe volver al login.</summary>
        public bool RequiereNuevoLogin { get; private set; }

        private static readonly (ModuloApp Modulo, int IconCode, string Label)[] DefinicionModulos =
        [
            (ModuloApp.Vehiculos, 0xE804, "Gestión de Flota"),
            (ModuloApp.Clientes, 0xE716, "Clientes"),
            (ModuloApp.Reservas, 0xE787, "Reservas"),
            (ModuloApp.Rentas, 0xE8F1, "Rentas Activas"),
            (ModuloApp.Mantenimiento, 0xE713, "Mantenimiento"),
            (ModuloApp.Facturacion, 0xE8A5, "Facturación"),
            (ModuloApp.Usuarios, 0xE77B, "Usuarios")
        ];

        /// <summary>Constructor sin parámetros para el diseñador de Visual Studio.</summary>
        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public MainForm(DashboardService dashboard) : this()
        {
            _dashboard = dashboard;

            // En tiempo de diseño el diseñador solo pinta lo declarado en MainForm.Designer.cs.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            RemoveDesignTimeHintPanel();
            BackColor = _mainBg;
            BuildDashboard();
        }

        private void RemoveDesignTimeHintPanel()
        {
            if (!Controls.Contains(designTimeHint))
                return;
            Controls.Remove(designTimeHint);
            designTimeHint.Dispose();
        }

        private void BuildDashboard()
        {
            _pnlSidebar = new Guna2Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                FillColor = _sidebarBg,
                BorderRadius = 0,
                Padding = new Padding(20, 28, 20, 24)
            };

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 4, 0, 12),
                BackColor = Color.Transparent
            };
            var lblBrand = new Label
            {
                Text = "RentCar Pro",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            var lblSubBrand = new Label
            {
                Text = "Sistema de Gestión",
                Font = new Font("Segoe UI", 9F),
                ForeColor = _textMuted,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            pnlHeader.Controls.Add(lblBrand);
            pnlHeader.Controls.Add(lblSubBrand);
            pnlHeader.Layout += (_, _) =>
            {
                lblSubBrand.Top = lblBrand.Bottom + 6;
            };

            var pnlNavHost = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 4, 0, 12),
                BackColor = Color.Transparent
            };

            var btnInicio = CreateNavButton("Inicio", 0xE80F, NavDashboard);
            btnInicio.Margin = new Padding(0, 0, 0, 8);
            btnInicio.Height = 52;
            pnlNavHost.Controls.Add(btnInicio);
            _navButtons.Add(btnInicio);

            var modulosPermitidos = UsuarioRoles.ModulosPermitidos(SesionActual.Usuario?.Rol);
            foreach (var def in DefinicionModulos)
            {
                if (!modulosPermitidos.Contains(def.Modulo))
                    continue;
                var btn = CreateNavButton(def.Label, def.IconCode, (int)def.Modulo);
                btn.Margin = new Padding(0, 0, 0, 8);
                btn.Height = 52;
                pnlNavHost.Controls.Add(btn);
                _navButtons.Add(btn);
            }

            SetSelectedNav(NavDashboard);

            var pnlProfile = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                MinimumSize = new Size(0, 108),
                FillColor = Color.FromArgb(30, 41, 59),
                BorderRadius = 12,
                Margin = new Padding(0, 12, 0, 0)
            };

            var avatar = new Guna2Panel
            {
                Size = new Size(40, 40),
                Location = new Point(12, 12),
                FillColor = _accentBlue,
                BorderRadius = 20
            };
            var u = SesionActual.Usuario!;
            var iniciales = ObtenerIniciales(u.Nombre, u.Apellido);
            _lblProfileAvatar = new Label
            {
                Text = iniciales,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = avatar.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            avatar.Controls.Add(_lblProfileAvatar);

            _lblProfileUser = new Label
            {
                Text = $"{u.Nombre} {u.Apellido}".Trim(),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(60, 14),
                AutoSize = true
            };
            _lblProfileMail = new Label
            {
                Text = $"{SesionActual.Rol} · {u.Email}",
                Font = new Font("Segoe UI", 8F),
                ForeColor = _textMuted,
                Location = new Point(60, 34),
                AutoSize = true
            };

            var btnCerrarSesion = new Guna2Button
            {
                Text = "Cerrar sesión",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(203, 213, 225),
                FillColor = Color.FromArgb(51, 65, 85),
                BorderRadius = 8,
                BorderThickness = 0,
                Height = 34,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            btnCerrarSesion.HoverState.FillColor = Color.FromArgb(71, 85, 105);
            btnCerrarSesion.Click += (_, _) => CerrarSesion();

            pnlProfile.Controls.Add(avatar);
            pnlProfile.Controls.Add(_lblProfileUser);
            pnlProfile.Controls.Add(_lblProfileMail);
            pnlProfile.Controls.Add(btnCerrarSesion);
            void LayoutProfileFooter()
            {
                var w = Math.Max(120, pnlProfile.ClientSize.Width - 24);
                btnCerrarSesion.Width = w;
                btnCerrarSesion.Left = 12;
                btnCerrarSesion.Top = Math.Max(58, pnlProfile.ClientSize.Height - btnCerrarSesion.Height - 10);
            }

            pnlProfile.Resize += (_, _) => LayoutProfileFooter();
            pnlProfile.HandleCreated += (_, _) => LayoutProfileFooter();

            var sidebarLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            sidebarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sidebarLayout.RowCount = 3;
            pnlHeader.AutoSize = true;
            pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            sidebarLayout.Controls.Add(pnlHeader, 0, 0);
            sidebarLayout.Controls.Add(pnlNavHost, 0, 1);
            sidebarLayout.Controls.Add(pnlProfile, 0, 2);
            _pnlSidebar.Controls.Add(sidebarLayout);

            void SyncNavButtonWidths()
            {
                var w = pnlNavHost.ClientSize.Width - pnlNavHost.Padding.Horizontal;
                if (w < 80)
                    w = 200;
                foreach (Guna2Button b in _navButtons)
                    b.Width = w;
            }

            pnlNavHost.Resize += (_, _) => SyncNavButtonWidths();
            _pnlSidebar.Resize += (_, _) => SyncNavButtonWidths();

            _pnlMain = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = _mainBg,
                BorderRadius = 0,
                Padding = new Padding(32, 28, 32, 24),
                AutoScroll = true
            };

            var headerTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 6)
            };
            var headerSub = new Label
            {
                Text = $"Bienvenido, {SesionActual.Usuario!.Nombre} — rol: {SesionActual.Rol}",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true
            };

            var cardsRow = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                Dock = DockStyle.Top,
                Height = 302,
                Margin = new Padding(0, 16, 0, 0),
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };
            cardsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            var kpi1 = CreateSummaryCard(
                "Vehículos Activos", "—", "Cargando…", Color.FromArgb(34, 197, 94),
                Mdl2Char(0xE7C1));
            cardsRow.Controls.Add(kpi1.Outer, 0, 0);
            _kpiVehVal = kpi1.Value;
            _kpiVehSub = kpi1.Sub;

            var kpi2 = CreateSummaryCard(
                "Rentas Próximas a Vencer", "—", "Cargando…", Color.FromArgb(249, 115, 22),
                Mdl2Char(0xE916), subForeColor: Color.FromArgb(239, 68, 68));
            cardsRow.Controls.Add(kpi2.Outer, 1, 0);
            _kpiRentVal = kpi2.Value;
            _kpiRentSub = kpi2.Sub;

            var kpi3 = CreateSummaryCard(
                "Mantenimientos abiertos", "—", "Cargando…", Color.FromArgb(234, 88, 12),
                Mdl2Char(0xE946), subForeColor: Color.FromArgb(239, 68, 68));
            cardsRow.Controls.Add(kpi3.Outer, 2, 0);
            _kpiMantVal = kpi3.Value;
            _kpiMantSub = kpi3.Sub;

            var lblQuick = new Label
            {
                Text = "Accesos Rápidos",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(0, 0),
                Margin = new Padding(0, 20, 0, 0)
            };

            var quickGrid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 12, 0, 0)
            };
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));

            var quickCards = new List<(string Titulo, string Desc, string Icono, Action Accion)>();
            if (SesionActual.PuedeAcceder(ModuloApp.Rentas))
                quickCards.Add(("Nueva Renta", "Registrar una nueva renta de vehículo", Mdl2Char(0xE710),
                    () => Program.ServiceProvider.GetRequiredService<RentaForm>().ShowDialog(this)));
            if (SesionActual.PuedeAcceder(ModuloApp.Clientes))
                quickCards.Add(("Registrar Cliente", "Añadir un nuevo cliente al sistema", Mdl2Char(0xE716),
                    () => Program.ServiceProvider.GetRequiredService<ClienteForm>().ShowDialog(this)));
            if (SesionActual.PuedeAcceder(ModuloApp.Vehiculos))
                quickCards.Add(("Ver Disponibilidad", "Consultar vehículos disponibles", Mdl2Char(0xE7B3),
                    () => ShowSingletonModule(() => Program.ServiceProvider.GetRequiredService<VehiculoList>())));
            if (SesionActual.PuedeAcceder(ModuloApp.Mantenimiento))
                quickCards.Add(("Estado de Mantenimiento", "Revisar mantenimientos programados", Mdl2Char(0xE713),
                    () => ShowSingletonModule(() => Program.ServiceProvider.GetRequiredService<MantenimientoList>())));

            for (var i = 0; i < quickCards.Count; i++)
            {
                var q = quickCards[i];
                quickGrid.Controls.Add(CreateQuickCard(q.Titulo, q.Desc, q.Icono, q.Accion), i % 2, i / 2);
            }

            var quickWrap = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                Width = _pnlMain.ClientSize.Width - 64
            };
            quickWrap.Controls.Add(lblQuick);
            quickWrap.Controls.Add(quickGrid);
            lblQuick.Location = new Point(0, 0);
            quickGrid.Location = new Point(0, 32);
            quickGrid.Width = quickWrap.Width;

            var lblAlerts = new Label
            {
                Text = "Alertas y Notificaciones",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                Location = new Point(16, 14)
            };

            var pnlAlertsBox = new Guna2Panel
            {
                FillColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                ShadowDecoration = { Enabled = true, Depth = 4, Color = Color.FromArgb(40, 0, 0, 0) },
                AutoSize = true,
                Padding = new Padding(4, 4, 4, 8)
            };

            var flpAlerts = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = 800,
                BackColor = Color.Transparent,
                Padding = new Padding(12, 12, 12, 16)
            };
            _flpAlerts = flpAlerts;

            pnlAlertsBox.Controls.Add(lblAlerts);
            pnlAlertsBox.Controls.Add(flpAlerts);
            lblAlerts.Left = 20;
            lblAlerts.Top = 16;
            flpAlerts.Left = 12;
            flpAlerts.Top = 48;
            flpAlerts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlAlertsBox.Resize += (_, _) =>
            {
                flpAlerts.Width = pnlAlertsBox.ClientSize.Width - 16;
            };

            var alertsOuter = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 24, 0, 0),
                Dock = DockStyle.Top
            };
            alertsOuter.Controls.Add(pnlAlertsBox);
            pnlAlertsBox.Dock = DockStyle.Top;

            var contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(0)
            };

            var stack = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            stack.Controls.Add(cardsRow);
            stack.Controls.Add(quickWrap);
            stack.Controls.Add(alertsOuter);

            cardsRow.Dock = DockStyle.Top;
            quickWrap.Dock = DockStyle.Top;
            alertsOuter.Dock = DockStyle.Top;

            contentHost.Controls.Add(stack);
            stack.Dock = DockStyle.Top;

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 0, 0, 12),
                BackColor = Color.Transparent
            };
            var headerStack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            headerStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            headerStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            headerStack.Controls.Add(headerTitle, 0, 0);
            headerStack.Controls.Add(headerSub, 0, 1);
            headerPanel.Controls.Add(headerStack);
            headerPanel.Layout += (_, _) =>
            {
                var w = headerPanel.ClientSize.Width;
                if (w <= 0)
                    return;
                headerSub.MaximumSize = new Size(w, 0);
                headerTitle.MaximumSize = new Size(w, 0);
            };

            _pnlMain.Controls.Add(contentHost);
            _pnlMain.Controls.Add(headerPanel);

            Controls.Add(_pnlMain);
            Controls.Add(_pnlSidebar);

            Shown += async (_, _) =>
            {
                SyncNavButtonWidths();
                quickWrap.Width = _pnlMain.ClientSize.Width - _pnlMain.Padding.Horizontal;
                quickGrid.Width = quickWrap.Width;
                flpAlerts.Width = Math.Max(flpAlerts.Width, _pnlMain.ClientSize.Width - _pnlMain.Padding.Horizontal - 24);
                await RefreshDashboardAsync();
            };
        }

        private async Task RefreshDashboardAsync()
        {
            if (_dashboard is null || _kpiVehVal is null || _flpAlerts is null)
                return;

            try
            {
                var d = await _dashboard.ObtenerAsync();

                _kpiVehVal.Text = d.VehiculosActivos.ToString();
                _kpiVehSub!.Text = d.VehiculosAltaEsteMes > 0
                    ? $"+{d.VehiculosAltaEsteMes} registrados este mes"
                    : "Sin altas este mes";
                _kpiVehSub.ForeColor = d.VehiculosAltaEsteMes > 0
                    ? Color.FromArgb(34, 197, 94)
                    : Color.FromArgb(100, 116, 139);

                _kpiRentVal!.Text = d.RentasProximasAVencer.ToString();
                _kpiRentSub!.Text = d.RentasProximasAVencer > 0
                    ? "Fin programado hoy o mañana (sin cierre)"
                    : "Ninguna en ventana inmediata";
                _kpiRentSub.ForeColor = d.RentasProximasAVencer > 0
                    ? Color.FromArgb(239, 68, 68)
                    : Color.FromArgb(100, 116, 139);

                _kpiMantVal!.Text = d.MantenimientosAbiertos.ToString();
                _kpiMantSub!.Text = d.MantenimientosAbiertos > 0
                    ? "Sin fecha de cierre registrada"
                    : "Sin mantenimientos abiertos";
                _kpiMantSub.ForeColor = d.MantenimientosAbiertos > 0
                    ? Color.FromArgb(239, 68, 68)
                    : Color.FromArgb(100, 116, 139);

                _flpAlerts.Controls.Clear();
                if (d.Alertas.Count == 0)
                {
                    var vacío = new Label
                    {
                        Text = "No hay alertas recientes.",
                        Font = new Font("Segoe UI", 9.5F),
                        ForeColor = Color.FromArgb(100, 116, 139),
                        AutoSize = true,
                        Padding = new Padding(8, 12, 8, 8)
                    };
                    _flpAlerts.Controls.Add(vacío);
                }
                else
                {
                    foreach (var a in d.Alertas)
                        _flpAlerts.Controls.Add(CreateAlertRowFromItem(a));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los datos del panel: {ex.Message}", "Dashboard",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Control CreateAlertRowFromItem(DashboardAlertaItem a)
        {
            var (color, icon) = a.Tipo switch
            {
                DashboardAlertaTipo.Peligro => (Color.FromArgb(239, 68, 68), Mdl2Char(0xE814)),
                DashboardAlertaTipo.Advertencia => (Color.FromArgb(249, 115, 22), Mdl2Char(0xE916)),
                _ => (Color.FromArgb(59, 130, 246), Mdl2Char(0xE713))
            };
            return CreateAlertRow(color, icon, a.Titulo, a.Detalle, a.Plazo);
        }

        private static string Mdl2Char(int codepoint) =>
            char.ConvertFromUtf32(codepoint);

        private Guna2Button CreateNavButton(string text, int mdl2IconCode, int index)
        {
            var btn = new Guna2Button
            {
                Text = string.Empty,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BorderRadius = 8,
                BorderThickness = 0,
                FillColor = Color.Transparent,
                HoverState = { FillColor = Color.FromArgb(51, 65, 85) },
                TextAlign = HorizontalAlignment.Left,
                AnimatedGIF = false,
                Cursor = Cursors.Hand,
                Tag = index
            };
            var lblIcon = new Label
            {
                Text = Mdl2Char(mdl2IconCode),
                Font = new Font("Segoe MDL2 Assets", 12F),
                ForeColor = Color.FromArgb(203, 213, 225),
                AutoSize = false,
                Size = new Size(36, 44),
                Location = new Point(12, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            var lblText = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(160, 44),
                Location = new Point(48, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btn.Controls.Add(lblIcon);
            btn.Controls.Add(lblText);
            void SyncNavBtnLayout()
            {
                lblText.Width = Math.Max(80, btn.ClientSize.Width - 56);
                var y = Math.Max(0, (btn.ClientSize.Height - lblIcon.Height) / 2);
                lblIcon.Top = y;
                lblText.Top = y;
            }
            btn.Resize += (_, _) => SyncNavBtnLayout();
            btn.HandleCreated += (_, _) => SyncNavBtnLayout();
            void forwardClick(object? s, EventArgs e) => NavButton_Click(btn, e);
            lblIcon.Click += forwardClick;
            lblText.Click += forwardClick;
            btn.Click += forwardClick;
            return btn;
        }

        private void NavButton_Click(object? sender, EventArgs e)
        {
            if (_navClickEnCurso)
                return;
            _navClickEnCurso = true;
            try
            {
                var btn = sender as Guna2Button ?? (sender as Control)?.Parent as Guna2Button;
                if (btn?.Tag is not int idx)
                    return;
                if (idx == NavDashboard)
                {
                    MostrarDashboard();
                    return;
                }

                SetSelectedNav(idx);
                OpenModule(idx);
            }
            finally
            {
                _navClickEnCurso = false;
            }
        }

        /// <summary>
        /// Evita ventanas duplicadas si el usuario hace doble clic o varios clics seguidos en el menú o accesos rápidos.
        /// </summary>
        private void MostrarDashboard()
        {
            SetSelectedNav(NavDashboard);
            OcultarModulosAbiertos();
            _pnlMain?.BringToFront();
            BringToFront();
            Activate();
            _ = RefreshDashboardAsync();
        }

        private void OcultarModulosAbiertos()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f == this || f.IsDisposed)
                    continue;
                if (f.Owner == this)
                    f.Hide();
            }
        }

        private void CerrarSesion()
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Cerrar sesión",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            SesionActual.Cerrar();
            RequiereNuevoLogin = true;

            foreach (Form f in Application.OpenForms.Cast<Form>().ToList())
            {
                if (f == this || f.IsDisposed)
                    continue;
                try
                {
                    f.Close();
                }
                catch
                {
                    // ignorar cierre de formularios ya destruidos
                }
            }

            Close();
        }

        private void ShowSingletonModule<T>(Func<T> create) where T : Form
        {
            try
            {
                foreach (Form open in Application.OpenForms)
                {
                    if (open is not T existing || existing.IsDisposed)
                        continue;
                    if (existing.WindowState == FormWindowState.Minimized)
                        existing.WindowState = FormWindowState.Normal;
                    // No volver a llamar a Show en un formulario ya visible: evita estados inválidos y
                    // conflictos con ShowDialog en la misma bomba de mensajes.
                    if (existing.Visible)
                    {
                        existing.BringToFront();
                        existing.Activate();
                        return;
                    }

                    existing.Show(this);
                    existing.BringToFront();
                    existing.Activate();
                    return;
                }

                var f = create();
                f.FormClosed += (_, _) =>
                {
                    if (!TieneModuloVisible())
                        SetSelectedNav(NavDashboard);
                };
                f.Show(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ShowSingletonModule: {ex}");
            }
        }

        private bool TieneModuloVisible()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f == this || f.IsDisposed || f.Owner != this)
                    continue;
                if (f.Visible)
                    return true;
            }

            return false;
        }

        private void OpenModule(int index)
        {
            if (_abriendoModulo)
                return;
            if (!Enum.IsDefined(typeof(ModuloApp), index))
                return;

            var modulo = (ModuloApp)index;
            if (!SesionActual.PuedeAcceder(modulo))
            {
                MessageBox.Show("Su rol no tiene permiso para abrir este módulo.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _abriendoModulo = true;
            try
            {
                SetSelectedNav(index);
                var sp = Program.ServiceProvider;
                switch (modulo)
                {
                    case ModuloApp.Vehiculos:
                        ShowSingletonModule(() => sp.GetRequiredService<VehiculoList>());
                        break;
                    case ModuloApp.Clientes:
                        ShowSingletonModule(() => sp.GetRequiredService<ClienteList>());
                        break;
                    case ModuloApp.Reservas:
                        ShowSingletonModule(() => sp.GetRequiredService<ReservaList>());
                        break;
                    case ModuloApp.Rentas:
                        ShowSingletonModule(() => sp.GetRequiredService<RentaList>());
                        break;
                    case ModuloApp.Mantenimiento:
                        ShowSingletonModule(() => sp.GetRequiredService<MantenimientoList>());
                        break;
                    case ModuloApp.Facturacion:
                        ShowSingletonModule(() => sp.GetRequiredService<FacturaList>());
                        break;
                    case ModuloApp.Usuarios:
                        ShowSingletonModule(() => sp.GetRequiredService<UsuarioList>());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OpenModule: {ex}");
            }
            finally
            {
                _abriendoModulo = false;
            }
        }

        private static string ObtenerIniciales(string nombre, string apellido)
        {
            var a = string.IsNullOrWhiteSpace(nombre) ? "" : nombre.Trim()[0].ToString().ToUpperInvariant();
            var b = string.IsNullOrWhiteSpace(apellido) ? "" : apellido.Trim()[0].ToString().ToUpperInvariant();
            var t = a + b;
            return string.IsNullOrEmpty(t) ? "?" : t;
        }

        private void SetSelectedNav(int moduloIndex)
        {
            for (var i = 0; i < _navButtons.Count; i++)
            {
                var b = _navButtons[i];
                var sel = b.Tag is int idx && idx == moduloIndex;
                b.FillColor = sel ? _accentBlue : Color.Transparent;
                b.ForeColor = Color.White;
                b.HoverState.FillColor = sel ? _accentBlue : Color.FromArgb(51, 65, 85);
                foreach (Control c in b.Controls)
                {
                    if (c is not Label l)
                        continue;
                    var isIcon = string.Equals(l.Font?.FontFamily.Name, "Segoe MDL2 Assets", StringComparison.OrdinalIgnoreCase);
                    l.ForeColor = isIcon
                        ? (sel ? Color.White : Color.FromArgb(203, 213, 225))
                        : Color.White;
                }
            }
        }

        private (Control Outer, Label Value, Label Sub) CreateSummaryCard(string title, string value, string sub,
            Color iconBg, string iconChar, Color? subForeColor = null)
        {
            var outer = new Panel
            {
                Padding = new Padding(0, 0, 8, 0),
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                Padding = new Padding(18, 16, 14, 22),
                Margin = new Padding(0),
                MinimumSize = new Size(0, 248),
                ShadowDecoration = { Enabled = true, Depth = 6, Color = Color.FromArgb(35, 0, 0, 0) }
            };

            var textPane = new Panel
            {
                BackColor = Color.Transparent
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                UseMnemonic = false
            };

            var lblVal = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                UseMnemonic = false
            };

            var subColor = subForeColor ?? (sub.StartsWith('+') ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68));
            var lblSub = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 9F),
                ForeColor = subColor,
                AutoSize = true,
                UseMnemonic = false
            };

            textPane.Controls.Add(lblTitle);
            textPane.Controls.Add(lblVal);
            textPane.Controls.Add(lblSub);

            var iconPanel = new Guna2Panel
            {
                Size = new Size(46, 46),
                FillColor = iconBg,
                BorderRadius = 11,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var fontMdl = new Font("Segoe MDL2 Assets", 17F, FontStyle.Regular, GraphicsUnit.Point);
            var lblIcon = new Label
            {
                Text = iconChar,
                Font = fontMdl,
                ForeColor = Color.White,
                AutoSize = false,
                Size = iconPanel.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconPanel.Controls.Add(lblIcon);

            const int iconColumn = 50;
            const int gapTextIcon = 10;

            void RelayoutCard()
            {
                if (!card.IsHandleCreated || card.ClientSize.Width <= 0)
                    return;

                var padL = card.Padding.Left;
                var padR = card.Padding.Right;
                var padT = card.Padding.Top;
                var padB = card.Padding.Bottom;
                var cw = card.ClientSize.Width;
                var ch = card.ClientSize.Height;

                var textW = cw - padL - padR - iconColumn - gapTextIcon;
                if (textW < 120)
                    textW = 120;

                lblTitle.MaximumSize = new Size(textW, 0);
                lblSub.MaximumSize = new Size(textW, 0);

                var y = 0;
                lblTitle.Location = new Point(0, y);
                y += lblTitle.GetPreferredSize(new Size(textW, int.MaxValue)).Height + 8;

                lblVal.Location = new Point(0, y);
                y += lblVal.GetPreferredSize(new Size(textW, int.MaxValue)).Height + 10;

                lblSub.Location = new Point(0, y);
                y += lblSub.GetPreferredSize(new Size(textW, int.MaxValue)).Height + 4;

                var innerAvailH = Math.Max(1, ch - padT - padB);
                textPane.SetBounds(padL, padT, textW, innerAvailH);
                textPane.AutoScroll = y > innerAvailH;
                iconPanel.Location = new Point(cw - padR - iconPanel.Width, padT);
            }

            void WireRelayout(Label L) => L.TextChanged += (_, _) => RelayoutCard();

            WireRelayout(lblTitle);
            WireRelayout(lblVal);
            WireRelayout(lblSub);

            card.Controls.Add(textPane);
            card.Controls.Add(iconPanel);
            card.Resize += (_, _) => RelayoutCard();
            card.HandleCreated += (_, _) => RelayoutCard();

            outer.Controls.Add(card);
            return (outer, lblVal, lblSub);
        }

        private Control CreateQuickCard(string title, string desc, string iconChar, Action? onClick = null)
        {
            var wrap = new Panel
            {
                Padding = new Padding(0, 0, 12, 14),
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                MinimumSize = new Size(220, 96)
            };

            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(226, 232, 240),
                Cursor = Cursors.Hand,
                Padding = new Padding(4, 4, 4, 4),
                ShadowDecoration = { Enabled = true, Depth = 3, Color = Color.FromArgb(25, 0, 0, 0) }
            };

            var iconBox = new Guna2Panel
            {
                Size = new Size(48, 48),
                Location = new Point(16, 20),
                FillColor = Color.FromArgb(224, 242, 254),
                BorderRadius = 10
            };
            var ic = new Label
            {
                Text = iconChar,
                Font = new Font("Segoe MDL2 Assets", 16F),
                ForeColor = Color.FromArgb(14, 165, 233),
                AutoSize = false,
                Size = iconBox.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            iconBox.Controls.Add(ic);

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = _textDark,
                AutoSize = true,
                MaximumSize = new Size(320, 0),
                Location = new Point(76, 18),
                Cursor = Cursors.Hand
            };
            var lblD = new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = false,
                Size = new Size(320, 44),
                Location = new Point(76, 44),
                Cursor = Cursors.Hand
            };

            card.Controls.Add(iconBox);
            card.Controls.Add(lblT);
            card.Controls.Add(lblD);
            wrap.Controls.Add(card);

            if (onClick != null)
            {
                void invoke(object? _, EventArgs __) => onClick();
                card.Click += invoke;
                wrap.Click += invoke;
                lblT.Click += invoke;
                lblD.Click += invoke;
                ic.Click += invoke;
                iconBox.Click += invoke;
            }

            return wrap;
        }

        private Control CreateAlertRow(Color accent, string iconChar, string title, string detail, string when)
        {
            var row = new Panel
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(4, 10, 12, 18),
                MinimumSize = new Size(400, 88),
                Width = 760
            };

            var iconCircle = new Guna2Panel
            {
                Size = new Size(44, 44),
                Location = new Point(8, 12),
                FillColor = Blend(accent, Color.White, 0.88f),
                BorderRadius = 22
            };

            var li = new Label
            {
                Text = iconChar,
                Font = new Font("Segoe MDL2 Assets", 15F),
                ForeColor = accent,
                AutoSize = false,
                Size = iconCircle.Size,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconCircle.Controls.Add(li);

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = _textDark,
                Location = new Point(64, 10),
                AutoSize = true,
                MaximumSize = new Size(640, 0)
            };
            var lblD = new Label
            {
                Text = detail,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(64, 36),
                AutoSize = true,
                MaximumSize = new Size(640, 0)
            };
            var lblW = new Label
            {
                Text = when,
                Font = new Font("Segoe UI", 9F),
                ForeColor = _textMuted,
                Location = new Point(64, 62),
                AutoSize = true
            };

            row.Controls.Add(iconCircle);
            row.Controls.Add(lblT);
            row.Controls.Add(lblD);
            row.Controls.Add(lblW);

            return row;
        }

        private static Color Blend(Color c, Color with, float amount)
        {
            var r = (int)(c.R * (1 - amount) + with.R * amount);
            var g = (int)(c.G * (1 - amount) + with.G * amount);
            var b = (int)(c.B * (1 - amount) + with.B * amount);
            return Color.FromArgb(r, g, b);
        }
    }
}
