using fNbt;

namespace SchematicCreator.Generators
{
    internal interface IGenerator
    {
        NbtCompound Generate(bool[,] binary);
    }
}