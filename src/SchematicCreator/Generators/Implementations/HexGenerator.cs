using fNbt;
using SchematicCreator.Configuration;
using SchematicCreator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchematicCreator.Generators.Implementations
{
    internal class HexGenerator : IGenerator
    {
        private MemoryConfiguration _configuration;

        public HexGenerator(MemoryConfiguration configuration)
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

            throw new NotImplementedException("Too lazy to implement this right now");

            schematic.Add(new NbtByteArray("BlockData", blockData));

            return schematic;
        }
    }
}
