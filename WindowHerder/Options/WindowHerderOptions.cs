using System.Windows.Forms;
using WindowHerder.Keyboard;

namespace WindowHerder.Options
{
    public class WindowHerderOptions
    {
        public HotKeyDefinition SaveStateHotKey { get; set; }

        public HotKeyDefinition RestoreStateHotKey { get; set; }

        public bool ShowStartupMessage { get; set; }

        public bool ShowSavedMessage { get; set; }

        public bool ShowRestoredMessage { get; set; }

        public int BalloonTipLifespan { get; set; }

        public WindowHerderOptions()
        {
            // default save hotkey: Alt+F1
            SaveStateHotKey = new HotKeyDefinition
            {
                IncludeAltKey = true,
                IncludeCtrlKey = false,
                IncludeShiftKey = false,
                IncludeWindowsKey = false,
                KeyCode = Keys.F1
            };

            // default restore hotkey: Alt+F2
            RestoreStateHotKey = new HotKeyDefinition
            {
                IncludeAltKey = true,
                IncludeCtrlKey = false,
                IncludeShiftKey = false,
                IncludeWindowsKey = false,
                KeyCode = Keys.F2
            };

            ShowStartupMessage = true;
            ShowSavedMessage = true;
            ShowRestoredMessage = false;
            BalloonTipLifespan = 500;
        }
    }
}
