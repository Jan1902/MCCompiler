using SchematicCreator.Configuration;

namespace SchematicCreator.Generators
{
    internal interface IGeneratorProvider
    {
        IGenerator GetGenerator(MemoryConfiguration configuration);
    }
}