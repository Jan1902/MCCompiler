using fNbt;
using SchematicCreator.Configuration;
using SchematicCreator.Generators;
using SchematicCreator.Parsing;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace SchematicCreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // File Checks
            if(!File.Exists(args[0]))
            {
                Console.WriteLine("File '{0}' does not exist!", args[0]);
                return;
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("File '{0}' does not exist!", args[1]);
                return;
            }

            // Config
            IConfigurationManager configManager = new ConfigurationManager(args[1]);
            configManager.Load();

            // Content
            var content = File.ReadAllLines(args[0]);
            IParser parser = new Parser(configManager);
            var binary = parser.Parse(content);

            // Generation
            IGenerator generator = new GeneratorProvider().GetGenerator(configManager.Configuration);
            var schematic = generator.Generate(binary);

            // Save
            var file = new NbtFile(schematic);
            file.SaveToFile(args[0].Replace(args[0].Split('.').Last(), "schem"), NbtCompression.GZip);
        }
    }
}