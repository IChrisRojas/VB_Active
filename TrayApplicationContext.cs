using System.Drawing;
using System.Windows.Forms;

namespace StayAwake;

public class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;
    private readonly System.Windows.Forms.Timer _activityTimer;
    private bool _isActive = false;

    private readonly ToolStripMenuItem _activateMenuItem;
    private readonly ToolStripMenuItem _deactivateMenuItem;

    public TrayApplicationContext()
    {
        // Setup Context Menu
        _activateMenuItem = new ToolStripMenuItem("Activar", null, OnActivate);
        _deactivateMenuItem = new ToolStripMenuItem("Desactivar", null, OnDeactivate) { Enabled = false };
        var exitMenuItem = new ToolStripMenuItem("Salir", null, OnExit);

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.AddRange(new ToolStripItem[] { _activateMenuItem, _deactivateMenuItem, new ToolStripSeparator(), exitMenuItem });

        // Setup Notify Icon
        _notifyIcon = new NotifyIcon
        {
            Icon = CreateTrayIcon(Color.Gray),
            ContextMenuStrip = contextMenu,
            Text = "Stay Awake - Desactivado",
            Visible = true
        };

        _notifyIcon.DoubleClick += (s, e) => ToggleStatus();

        // Setup Timer
        _activityTimer = new System.Windows.Forms.Timer();
        _activityTimer.Interval = 240000; // 4 minutes
        _activityTimer.Tick += (s, e) => NativeMethods.SimulateF15KeyPress();
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
        _notifyIcon.Text = "Stay Awake - Activo (F15 cada 4m)";
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

    private void OnExit(object? sender, EventArgs e)
    {
        _activityTimer.Stop();
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
        }
        base.Dispose(disposing);
    }
}
