using fNbt;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace SchematicCreator
{
    public class Program
    {
        private const int WIDTH = 32;
        private const int HEIGHT = 16;
        private const int LENGTH = 32;

        private const int LINES_PER_BLOCK = 16;
        private const int BLOCK_COUNT = 2;
        private const int BLOCK_SPACING = 8;

        public static void Main(string[] args)
        {
            var binary = File.ReadAllLines(args[0]);

            var schematic = new NbtCompound("Schematic");

            // Dimensions
            schematic.Add(new NbtShort("Width", WIDTH));
            schematic.Add(new NbtShort("Height", HEIGHT));
            schematic.Add(new NbtShort("Length", LENGTH));

            // Other
            schematic.Add(new NbtInt("DataVersion", 2730));
            schematic.Add(new NbtInt("Version", 2));

            // WE
            var metadata = new NbtCompound("Metadata");
            metadata.Add(new NbtInt("WEOffsetX", 0));
            metadata.Add(new NbtInt("WEOffsetY", -16));
            metadata.Add(new NbtInt("WEOffsetZ", 0));
            schematic.Add(metadata);

            var offset = new NbtIntArray("Offset", new int[] { 0, -16, 0 });
            schematic.Add(offset);

            //// BlockEntities
            //var blockEntities = new NbtList("BlockEntities");
            //schematic.Add(blockEntities);

            // Palette
            var palete = new NbtCompound("Palette");
            palete.Add(new NbtInt("minecraft:air", 0));
            palete.Add(new NbtInt("minecraft:smooth_quartz", 1));
            palete.Add(new NbtInt("minecraft:redstone_block", 2));
            schematic.Add(palete);

            schematic.Add(new NbtInt("PaletteMax", 3));

            // Block Data
            var blockData = new byte[WIDTH * HEIGHT * LENGTH];
            for (int i = 0; i < WIDTH * HEIGHT * LENGTH; i++)
                blockData[i] = 0;

            var pageCount = (int)Math.Ceiling((double)binary.Length / (LINES_PER_BLOCK * BLOCK_COUNT));
            for (int page = 0; page < pageCount; page++)
            {
                var blockCount = (int)Math.Ceiling((double)binary.Length / LINES_PER_BLOCK);
                for (int block = 0; block < blockCount; block++)
                {
                    for (int z = 0; z < (binary.Length > LINES_PER_BLOCK ? LINES_PER_BLOCK : binary.Length); z++)
                    {
                        for (int i = 0; i < 8; i++)
                            blockData[CalculateIndex(BLOCK_SPACING * block, HEIGHT - 1 - i * 2, z * 2)] = binary[z].Replace(" ", "").ToCharArray()[i] == '1' ? (byte)2 : (byte)1;

                        for (int i = 0; i < 8; i++)
                            blockData[CalculateIndex(BLOCK_SPACING * block + BLOCK_COUNT * BLOCK_SPACING, HEIGHT - 1 - i * 2, z * 2)] = binary[z].Replace(" ", "").ToCharArray()[i + 8] == '1' ? (byte)2 : (byte)1;
                    }
                }

                continue; //ONLY ONE PAGE FOR NOW
            }

            schematic.Add(new NbtByteArray("BlockData", blockData));

            // Save
            var file = new NbtFile(schematic);
            file.SaveToFile("test.schem", NbtCompression.GZip);
        }

        private static int CalculateIndex(int x, int y, int z) => (y * LENGTH + z) * WIDTH + x;
    }
}