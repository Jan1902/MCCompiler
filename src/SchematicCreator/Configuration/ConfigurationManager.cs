using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchematicCreator.Configuration
{
    internal class ConfigurationManager : IConfigurationManager
    {
        public MemoryConfiguration Configuration { get; private set; }

        private readonly string _path;

        public ConfigurationManager(string path)
        {
            _path = path;
        }

        public void Load()
        {
            if (!File.Exists(_path))
                throw new Exception("Configuration file not found!");

            var content = File.ReadAllText(_path);

            Configuration = JsonSerializer.Deserialize<MemoryConfiguration>(content);
        }
    }
}
