using System.Drawing;
using System.Windows.Forms;

namespace StayAwake;

public class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;
    private readonly System.Windows.Forms.Timer _activityTimer;
    private readonly System.Windows.Forms.Timer _vpnDisconnectTimer;
    private bool _isActive = false;

    private readonly ToolStripMenuItem _activateMenuItem;
    private readonly ToolStripMenuItem _deactivateMenuItem;
    
    // VPN Menu Items
    private readonly ToolStripMenuItem _vpnConnectMenuItem;
    private readonly ToolStripMenuItem _vpnConnectTimedMenuItem;
    private readonly ToolStripMenuItem _vpnDisconnectMenuItem;

    public TrayApplicationContext()
    {
        // Setup VPN Menu Items
        _vpnConnectMenuItem = new ToolStripMenuItem("VPN: Conectar", null, OnVpnConnect);
        _vpnConnectTimedMenuItem = new ToolStripMenuItem("VPN: Conectar por 30 min", null, OnVpnConnectTimed);
        _vpnDisconnectMenuItem = new ToolStripMenuItem("VPN: Desconectar", null, OnVpnDisconnect);
        var vpnConfigMenuItem = new ToolStripMenuItem("Configurar VPN", null, OnVpnConfig);

        // Setup Main Menu Items
        _activateMenuItem = new ToolStripMenuItem("Activar Stay Awake", null, OnActivate);
        _deactivateMenuItem = new ToolStripMenuItem("Desactivar Stay Awake", null, OnDeactivate) { Enabled = false };
        var exitMenuItem = new ToolStripMenuItem("Salir", null, OnExit);

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.AddRange(new ToolStripItem[] { 
            _vpnConnectMenuItem, 
            _vpnConnectTimedMenuItem, 
            _vpnDisconnectMenuItem, 
            vpnConfigMenuItem,
            new ToolStripSeparator(),
            _activateMenuItem, 
            _deactivateMenuItem, 
            new ToolStripSeparator(), 
            exitMenuItem 
        });

        // Setup Notify Icon
        _notifyIcon = new NotifyIcon
        {
            Icon = CreateTrayIcon(Color.Gray),
            ContextMenuStrip = contextMenu,
            Text = "Stay Awake & VPN Manager",
            Visible = true
        };

        _notifyIcon.DoubleClick += (s, e) => ToggleStatus();

        // Setup Activity Timer
        _activityTimer = new System.Windows.Forms.Timer();
        _activityTimer.Interval = 240000; // 4 minutes
        _activityTimer.Tick += (s, e) => NativeMethods.SimulateF15KeyPress();

        // Setup VPN Disconnect Timer
        _vpnDisconnectTimer = new System.Windows.Forms.Timer();
        _vpnDisconnectTimer.Interval = 30 * 60 * 1000; // 30 minutes
        _vpnDisconnectTimer.Tick += async (s, e) => await PerformVpnDisconnect(true);
    }

    private void ToggleStatus()
    {
        if (_isActive) OnDeactivate(null, EventArgs.Empty);
        else OnActivate(null, EventArgs.Empty);
    }

    private void OnActivate(object? sender, EventArgs e)
    {
        _isActive = true;
        _activityTimer.Start();
        _notifyIcon.Icon = CreateTrayIcon(Color.Blue);
        _notifyIcon.Text = "Stay Awake - Activo";
        _activateMenuItem.Enabled = false;
        _deactivateMenuItem.Enabled = true;
    }

    private void OnDeactivate(object? sender, EventArgs e)
    {
        _isActive = false;
        _activityTimer.Stop();
        _notifyIcon.Icon = CreateTrayIcon(Color.Gray);
        _notifyIcon.Text = "Stay Awake - Desactivado";
        _activateMenuItem.Enabled = true;
        _deactivateMenuItem.Enabled = false;
    }

    private async void OnVpnConnect(object? sender, EventArgs e)
    {
        await PerformVpnConnect();
    }

    private async void OnVpnConnectTimed(object? sender, EventArgs e)
    {
        if (await PerformVpnConnect())
        {
            _vpnDisconnectTimer.Start();
            NotifyUser("VPN Timed Connection", "VPN connected for 30 minutes.");
        }
    }

    private async void OnVpnDisconnect(object? sender, EventArgs e)
    {
        await PerformVpnDisconnect(false);
    }

    private void OnVpnConfig(object? sender, EventArgs e)
    {
        using (var configForm = new VpnConfigForm())
        {
            configForm.ShowDialog();
        }
    }

    private async Task<bool> PerformVpnConnect()
    {
        var config = ConfigService.LoadConfig();
        if (string.IsNullOrEmpty(config.ProfileName))
        {
            NotifyUser("VPN Error", "VPN profile not configured. Please go to 'Configurar VPN'.", ToolTipIcon.Error);
            return false;
        }

        NotifyUser("VPN", "Connecting to " + config.ProfileName + "...");
        bool success = await VpnManager.Connect(config.ProfileName, config.Username, config.GetPassword());

        if (success)
        {
            NotifyUser("VPN Success", "Connected to " + config.ProfileName);
            return true;
        }
        else
        {
            NotifyUser("VPN Failure", "Could not connect to " + config.ProfileName, ToolTipIcon.Error);
            return false;
        }
    }

    private async Task PerformVpnDisconnect(bool isTimed)
    {
        _vpnDisconnectTimer.Stop();
        var config = ConfigService.LoadConfig();
        if (string.IsNullOrEmpty(config.ProfileName)) return;

        bool success = await VpnManager.Disconnect(config.ProfileName);
        if (success)
        {
            NotifyUser("VPN Disconnected", isTimed ? "30-minute timer expired. VPN disconnected." : "VPN connection closed.");
        }
    }

    private void NotifyUser(string title, string text, ToolTipIcon icon = ToolTipIcon.Info)
    {
        _notifyIcon.ShowBalloonTip(3000, title, text, icon);
    }

    private void OnExit(object? sender, EventArgs e)
    {
        _activityTimer.Stop();
        _vpnDisconnectTimer.Stop();
        _notifyIcon.Visible = false;
        Application.Exit();
    }

    private Icon CreateTrayIcon(Color color)
    {
        using var bitmap = new Bitmap(32, 32);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            using var brush = new SolidBrush(color);
            g.FillEllipse(brush, 4, 4, 24, 24);
            using var pen = new Pen(Color.White, 2);
            g.DrawEllipse(pen, 4, 4, 24, 24);
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _notifyIcon.Dispose();
            _activityTimer.Dispose();
            _vpnDisconnectTimer.Dispose();
        }
        base.Dispose(disposing);
    }
}
