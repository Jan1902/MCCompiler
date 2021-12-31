using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompilerTest.Configuration
{
    internal class ConfigurationManager : IConfigurationManager
    {
        public CPUConfiguration Configuration { get; private set; }

        public void Load(string path)
        {
            if (!File.Exists(path))
                throw new Exception("Config File was not found");

            var content = File.ReadAllText(path);

            Configuration = JsonSerializer.Deserialize<CPUConfiguration>(content);
        }
    }
}
