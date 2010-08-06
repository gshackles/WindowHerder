using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using WindowHerder.Keyboard;
using WindowHerder.Options;
using WindowHerder.Window;

namespace WindowHerder
{
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _notifyIcon;
        private IWindowStateManager _windowStateManager;
        private IHotKeyRegistrar _hotkeyRegistrar;
        private BaseOptionsManager _optionsProvider;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // can eventually set these through DI if we want
            _windowStateManager = new NativeWindowStateManager();
            _hotkeyRegistrar = new ManagedHotKeyRegistrar();
            _optionsProvider = new RegistryOptionsManager();

            registerHotKeys();
            setupTrayIcon();

            showBalloonTip(WindowHerder.Resources.Strings.StartupBalloonMessage, ToolTipIcon.Info, _optionsProvider.Options.ShowStartupMessage);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _notifyIcon.Dispose();
            _hotkeyRegistrar.UnregisterAllHotKeys();
            _optionsProvider.SaveOptions();
        }

        private void showBalloonTip(string message, ToolTipIcon icon, bool tipEnabled = true)
        {
            if (tipEnabled) 
            {
                _notifyIcon.ShowBalloonTip(_optionsProvider.Options.BalloonTipLifespan, WindowHerder.Resources.Strings.ApplicationTitle, message, icon);
            }
        }

        private void setupTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Text = WindowHerder.Resources.Strings.ApplicationTitle,
                Visible = true,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem(WindowHerder.Resources.Strings.SystemTrayMenu_TakeSnapshot, new EventHandler((s, ev) => saveWindowList())),
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
            _hotkeyRegistrar.RegisterHotKey(_optionsProvider.Options.SaveStateHotKey, saveWindowList);
            _hotkeyRegistrar.RegisterHotKey(_optionsProvider.Options.RestoreStateHotKey, restoreWindowList);
        }

        private void saveWindowList()
        {
            bool success = _windowStateManager.StoreVisibleWindowStates();

            if (success)
            {
                showBalloonTip(WindowHerder.Resources.Strings.SnapshotTaken_SuccessBalloonMessage, ToolTipIcon.Info, _optionsProvider.Options.ShowSavedMessage);
            }
            else
            {
                showBalloonTip(WindowHerder.Resources.Strings.SnapshotTaken_ErrorBalloonMessage, ToolTipIcon.Error, _optionsProvider.Options.ShowSavedMessage);
            }
        }

        private void restoreWindowList()
        {
            bool success = _windowStateManager.RestoreStoredWindowStates();

            showBalloonTip(WindowHerder.Resources.Strings.SnapshotRestored_SuccessBalloonMessage, ToolTipIcon.Info, success && _optionsProvider.Options.ShowRestoredMessage);
        }
    }
}
