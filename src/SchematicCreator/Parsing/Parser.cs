using SchematicCreator.Configuration;

namespace SchematicCreator.Parsing
{
    internal class Parser : IParser
    {
        private readonly IConfigurationManager _configurationManager;

        public Parser(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public bool[,] Parse(string[] content)
        {
            var binary = new bool[content.Length, _configurationManager.Configuration.InstructionSize];

            for (int l = 0; l < content.Length; l++)
            {
                for (int b = 0; b < _configurationManager.Configuration.InstructionSize / 8; b++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        binary[l, b * 8 + i] = content[l].Replace(" ", "")[b * 8 + i] == '1';
                    }
                }
            }

            return binary;
        }
    }
}
