using System.Windows.Forms;

namespace WindowHerder.Keyboard
{
    public delegate void HotKeyPressedCallback();

    public class HotKeyDefinition
    {
        public bool IncludeAltKey { get; set; }

        public bool IncludeCtrlKey { get; set; }

        public bool IncludeShiftKey { get; set; }

        public bool IncludeWindowsKey { get; set; }

        public Keys KeyCode { get; set; }

        public HotKeyPressedCallback Callback { get; set; }
    }
}
