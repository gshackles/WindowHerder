using System;
using System.Collections.Generic;
using ManagedWinapi;

namespace WindowHerder.Keyboard
{
    public class ManagedHotKeyRegistrar : IHotKeyRegistrar
    {
        private IList<Hotkey> _hotkeys;

        public ManagedHotKeyRegistrar()
        {
            _hotkeys = new List<Hotkey>();
        }

        public void RegisterHotKey(HotKeyDefinition definition)
        {
            try
            {
                var hotkey = new Hotkey
                {
                    Alt = definition.IncludeAltKey,
                    Ctrl = definition.IncludeCtrlKey,
                    Shift = definition.IncludeShiftKey,
                    WindowsKey = definition.IncludeWindowsKey,
                    KeyCode = definition.KeyCode
                };

                hotkey.HotkeyPressed += ((sender, e) => definition.Callback.Invoke());
                hotkey.Enabled = true;

                _hotkeys.Add(hotkey);
            }
            catch (Exception)
            {
            }
        }

        public void UnregisterAllHotKeys()
        {
            foreach (Hotkey hotkey in _hotkeys)
            {
                hotkey.Dispose();
            }
        }
    }
}
