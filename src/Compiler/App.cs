using CommandLine;
using CompilerTest.Compiling;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Other;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest
{
    internal class App
    {
        private readonly ILogger<App> _logger;

        public App(ILogger<App> logger)
        {
            _logger = logger;
        }

        public void Run(string[] args)
        {
            Options options = null;
            Parser.Default.ParseArguments<Options>(args).WithParsed((opts) => options = opts);

            if (options == null)
                return;

            _logger.LogInformation("");
            _logger.LogInformation("Compiler started");

            // Check files
            if (!File.Exists(options.CodeFile))
            {
                _logger.LogError("File '{0}' not found", options.CodeFile);
                return;
            }

            if (!File.Exists(options.InstructionSetFile))
            {
                _logger.LogError("File '{0}' not found", options.InstructionSetFile);
                return;
            }

            // Instruction Set
            _logger.LogInformation("Loading Instruction Set from file '{0}'", options.InstructionSetFile);

            var instructionSet = InstructionSetProvider.LoadInstructionSet(File.ReadAllText(options.InstructionSetFile));

            _logger.LogInformation("Successfully loaded Instruction Set with {0} instructions and {1} conditions", instructionSet.Instructions.Count, instructionSet.Conditions.Count);

            // Code
            _logger.LogInformation("Compiling file '{0}'", options.CodeFile);

            var sourceCode = File.ReadAllText(options.CodeFile);

            // Compiling
            var compiler = new Compiler(instructionSet);
            string[] result = null;

            //result = compiler.Compile(sourceCode);
            try
            {
                result = compiler.Compile(sourceCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("Compiler Error: " + ex.Message);
                return;
            }

            _logger.LogInformation("Done");

            // Output
            var output = "";
            foreach (var line in result)
            {
                output += string.Join("", line) + Environment.NewLine;
            }

            File.WriteAllText(options.OutputFile, output);
            _logger.LogInformation("Outputted Result into file '{0}'", options.OutputFile);
        }
    }
}
