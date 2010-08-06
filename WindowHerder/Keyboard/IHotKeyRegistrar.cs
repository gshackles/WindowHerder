namespace WindowHerder.Keyboard
{
    public interface IHotKeyRegistrar
    {
        void RegisterHotKey(HotKeyDefinition definition, HotKeyPressedCallback callback);
        void UnregisterAllHotKeys();
    }
}
