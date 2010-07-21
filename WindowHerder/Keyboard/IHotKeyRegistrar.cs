namespace WindowHerder.Keyboard
{
    public interface IHotKeyRegistrar
    {
        void RegisterHotKey(HotKeyDefinition definition);
        void UnregisterAllHotKeys();
    }
}
