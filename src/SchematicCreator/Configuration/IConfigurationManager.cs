namespace SchematicCreator.Configuration
{
    internal interface IConfigurationManager
    {
        MemoryConfiguration Configuration { get; }

        void Load();
    }
}