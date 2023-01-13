using SchematicCreator.Configuration;
using SchematicCreator.Generators.Implementations;

namespace SchematicCreator.Generators;

internal class GeneratorProvider : IGeneratorProvider
{
    public IGenerator GetGenerator(MemoryConfiguration configuration)
    {
        return configuration.Type switch
        {
            MemoryType.Binary => new BinaryGenerator(configuration),
            MemoryType.Hex => new HexGenerator(configuration),
            _ => throw new Exception("Couln't find a Generator compatible with the given memory type!")
        };
    }
}
