namespace WindowHerder.Window
{
    interface IWindowStateManager
    {
        bool StoreVisibleWindowStates();
        bool RestoreStoredWindowStates();
    }
}
