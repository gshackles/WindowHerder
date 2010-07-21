using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using WindowHerder.Keyboard;
using WindowHerder.Window;

namespace WindowHerder
{
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _notifyIcon;
        private IWindowStateManager _windowStateManager;
        private IHotKeyRegistrar _hotkeyRegistrar;
        private const int _balloonTipLifespan = 500;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // can eventually set these through DI if we want
            _windowStateManager = new NativeWindowStateManager();
            _hotkeyRegistrar = new ManagedHotKeyRegistrar();

            registerHotKeys();
            setupTrayIcon();

            _notifyIcon.ShowBalloonTip(_balloonTipLifespan, WindowHerder.Resources.Strings.ApplicationTitle, WindowHerder.Resources.Strings.StartupBalloonMessage, ToolTipIcon.Info);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _notifyIcon.Dispose();
            _hotkeyRegistrar.UnregisterAllHotKeys();
        }

        private void setupTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Text = WindowHerder.Resources.Strings.ApplicationTitle,
                Visible = true,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem(WindowHerder.Resources.Strings.SystemTrayMenu_TakeSnapshot, new EventHandler((s, ev) => refreshWindowList())),
                    new MenuItem(WindowHerder.Resources.Strings.SystemTrayMenu_RestoreSnapshot, new EventHandler((s, ev) => restoreWindowList())),
                    new MenuItem("-"), 
                    new MenuItem(WindowHerder.Resources.Strings.SystemTrayMenu_Exit, new EventHandler((s, ev) => this.Shutdown()))
                })
            };

            using (Stream stream = System.Windows.Application.GetResourceStream(new Uri("/WindowHerder.ico", UriKind.Relative)).Stream)
            {
                _notifyIcon.Icon = new System.Drawing.Icon(stream);
            }
        }

        private void registerHotKeys() 
        {
            _hotkeyRegistrar.RegisterHotKey(new HotKeyDefinition
            {
                IncludeAltKey = true,
                IncludeCtrlKey = false,
                IncludeShiftKey = false,
                IncludeWindowsKey = false,
                KeyCode = Keys.F1,
                Callback = refreshWindowList
            });

            _hotkeyRegistrar.RegisterHotKey(new HotKeyDefinition
            {
                IncludeAltKey = true,
                IncludeCtrlKey = false,
                IncludeShiftKey = false,
                IncludeWindowsKey = false,
                KeyCode = Keys.F2,
                Callback = restoreWindowList
            });
        }

        private void refreshWindowList()
        {
            bool success = _windowStateManager.StoreVisibleWindowStates();
            string message = success
                                ? WindowHerder.Resources.Strings.SnapshotTaken_SuccessBalloonMessage
                                : WindowHerder.Resources.Strings.SnapshotTaken_ErrorBalloonMessage;

            _notifyIcon.ShowBalloonTip(_balloonTipLifespan, WindowHerder.Resources.Strings.ApplicationTitle, message, ToolTipIcon.Info);
        }

        private void restoreWindowList()
        {
            bool success = _windowStateManager.RestoreStoredWindowStates();

            if (success)
            {
                _notifyIcon.ShowBalloonTip(_balloonTipLifespan, WindowHerder.Resources.Strings.ApplicationTitle, WindowHerder.Resources.Strings.SnapshotRestored_SuccessBalloonMessage, ToolTipIcon.Info);
            }
        }
    }
}
