namespace WindowHerder.Options
{
    public abstract class BaseOptionsManager : IOptionsManager
    {
        public WindowHerderOptions Options { get; set; }

        public BaseOptionsManager()
        {
            Options = new WindowHerderOptions();
        }

        public abstract void SaveOptions();
    }
}
