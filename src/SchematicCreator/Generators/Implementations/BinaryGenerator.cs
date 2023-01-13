using fNbt;
using SchematicCreator.Configuration;
using SchematicCreator.Utility;

namespace SchematicCreator.Generators.Implementations;

internal class BinaryGenerator : IGenerator
{
    private MemoryConfiguration _configuration;

    public BinaryGenerator(MemoryConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NbtCompound Generate(bool[,] binary)
    {
        var schematic = new NbtCompound("Schematic");

        // Dimensions
        var width = (short)(_configuration.SpacingX * _configuration.BlockOrPageCount);
        var height = (short)(_configuration.SpacingY * 8);
        var length = (short)(_configuration.SpacingZ * _configuration.CellsPerBlockOrPage);

        schematic.Add(new NbtShort("Width", width));
        schematic.Add(new NbtShort("Height", height));
        schematic.Add(new NbtShort("Length", length));

        // Other
        schematic.Add(new NbtInt("DataVersion", 2730));
        schematic.Add(new NbtInt("Version", 2));

        // WE
        var metadata = new NbtCompound("Metadata");
        metadata.Add(new NbtInt("WEOffsetX", 0));
        metadata.Add(new NbtInt("WEOffsetY", (short)(_configuration.SpacingY * 8) * -1));
        metadata.Add(new NbtInt("WEOffsetZ", 0));
        schematic.Add(metadata);

        var offset = new NbtIntArray("Offset", new int[] { 0, -16, 0 });
        schematic.Add(offset);

        // Palette
        var palete = new NbtCompound("Palette");
        palete.Add(new NbtInt("minecraft:air", 0));
        palete.Add(new NbtInt(_configuration.OffMaterial, 1));
        palete.Add(new NbtInt(_configuration.OnMaterial, 2));
        schematic.Add(palete);

        schematic.Add(new NbtInt("PaletteMax", palete.Count));

        // Block Data
        var blockData = new byte[width * height * length];
        for (int i = 0; i < width * height * length; i++)
            blockData[i] = 0;

        var pageCount = (int)Math.Ceiling((double)binary.GetLength(0) / (_configuration.CellsPerBlockOrPage * (_configuration.BlockOrPageCount / 2)));
        for (int page = 0; page < pageCount; page++)
        {
            var blockCount = (int)Math.Ceiling((double)binary.GetLength(0) / _configuration.CellsPerBlockOrPage);
            for (int block = 0; block < blockCount; block++)
            {
                var linesForThisBlock = Math.Min(binary.GetLength(0) - block * _configuration.CellsPerBlockOrPage, _configuration.CellsPerBlockOrPage);
                for (int z = 0; z < linesForThisBlock; z++)
                {
                    for (int b = 0; b < binary.GetLength(1) / 8; b++)
                    {
                        for (int i = 0; i < 8; i++)
                            blockData[Utils.CalculateIndex(
                                width,
                                length,
                                _configuration.SpacingX * block + (_configuration.BlockOrPageCount / 2) * _configuration.SpacingX * b,
                                height - 1 - i * _configuration.SpacingY,
                                z * _configuration.SpacingZ)] = binary[z + block * _configuration.CellsPerBlockOrPage, i + 8 * b] ? (byte)2 : (byte)1;
                    }
                }
            }

            break; //ONLY ONE PAGE FOR NOW
        }

        schematic.Add(new NbtByteArray("BlockData", blockData));

        return schematic;
    }
}
