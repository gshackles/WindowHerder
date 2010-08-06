using System;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowHerder.Keyboard;

namespace WindowHerder.Options
{
    public class RegistryOptionsManager : BaseOptionsManager
    {
        private const string _registryKeyName = @"Software\WindowHerder";

        public RegistryOptionsManager()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(_registryKeyName)) 
            {
                Options.BalloonTipLifespan = Convert.ToInt32(key.GetValue("BalloonTipLifespan", Options.BalloonTipLifespan));
                Options.ShowStartupMessage = Convert.ToBoolean(key.GetValue("ShowStartupMessage", Options.ShowStartupMessage));
                Options.ShowSavedMessage = Convert.ToBoolean(key.GetValue("ShowSavedMessage", Options.ShowStartupMessage));
                Options.ShowRestoredMessage = Convert.ToBoolean(key.GetValue("ShowRestoredMessage", Options.ShowStartupMessage));

                Options.SaveStateHotKey = loadHotKey(key, "SaveHotKey");
                Options.RestoreStateHotKey = loadHotKey(key, "RestoreHotKey");
            }
        }

        public override void SaveOptions()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(_registryKeyName))
            {
                key.SetValue("BalloonTipLifespan", Options.BalloonTipLifespan);
                key.SetValue("ShowStartupMessage", Options.ShowStartupMessage);
                key.SetValue("ShowSavedMessage", Options.ShowSavedMessage);
                key.SetValue("ShowRestoredMessage", Options.ShowRestoredMessage);

                saveHotKey(key, "SaveHotKey", Options.SaveStateHotKey);
                saveHotKey(key, "RestoreHotKey", Options.RestoreStateHotKey);
            }
        }

        private void saveHotKey(RegistryKey parentRegistryKey, string subRegistryKeyName, HotKeyDefinition hotKey)
        {
            using (RegistryKey subKey = parentRegistryKey.CreateSubKey(subRegistryKeyName)) 
            {
                subKey.SetValue("IncludeAltKey", hotKey.IncludeAltKey);
                subKey.SetValue("IncludeCtrlKey", hotKey.IncludeCtrlKey);
                subKey.SetValue("IncludeShiftKey", hotKey.IncludeShiftKey);
                subKey.SetValue("IncludeWindowsKey", hotKey.IncludeWindowsKey);
                subKey.SetValue("KeyCode", (int)hotKey.KeyCode);
            }
        }

        private HotKeyDefinition loadHotKey(RegistryKey parentRegistryKey, string subRegistryKeyName)
        {
            var hotKey = new HotKeyDefinition();

            using (RegistryKey subKey = parentRegistryKey.CreateSubKey(subRegistryKeyName))
            {
                hotKey.IncludeAltKey = Convert.ToBoolean(subKey.GetValue("IncludeAltKey", hotKey.IncludeAltKey));
                hotKey.IncludeCtrlKey = Convert.ToBoolean(subKey.GetValue("IncludeCtrlKey", hotKey.IncludeCtrlKey));
                hotKey.IncludeShiftKey = Convert.ToBoolean(subKey.GetValue("IncludeShiftKey", hotKey.IncludeShiftKey));
                hotKey.IncludeWindowsKey = Convert.ToBoolean(subKey.GetValue("IncludeWindowsKey", hotKey.IncludeWindowsKey));
                hotKey.KeyCode = (Keys)Convert.ToInt32(subKey.GetValue("KeyCode", hotKey.KeyCode));
            }

            return hotKey;
        }
    }
}
