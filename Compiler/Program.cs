using CompilerTest.Compiling;
using CompilerTest.Compiling.Advanced;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Other.Logging;
using System;
using System.IO;

namespace CompilerTest
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            _logger = new Logger();
            _logger.LogInfo("");
            _logger.LogInfo("Compiler started");

            // Params

            if(args.Length < 4)
            {
                _logger.LogError("Invalid Arguments! Closing...");
                return;
            }

            var compilerType = args[0];
            var codeFileName = args[1];
            var instructionSetFileName = args[2];
            var resultFileName = args[3];

            // Check files

            if(!File.Exists(codeFileName))
            {
                _logger.LogError("File '{0}' not found", codeFileName);
                return;
            }

            if (!File.Exists(instructionSetFileName))
            {
                _logger.LogError("File '{0}' not found", instructionSetFileName);
                return;
            }

            // Instruction Set

            _logger.LogInfo("Loading Instruction Set from file '{0}'", instructionSetFileName);

            var instructionSet = InstructionSetProvider.LoadInstructionSet(File.ReadAllText(instructionSetFileName));

            _logger.LogSuccess("Successfully loaded Instruction Set with {0} instructions", instructionSet.Instructions.Count);

            // Code

            Console.WriteLine();

            _logger.LogInfo("Compiling file '{0}'", codeFileName);

            var sourceCode = File.ReadAllText(codeFileName);

            _logger.LogInfo("Loaded source code from file '{0}': \n\n{1}\n", codeFileName, sourceCode);

            // Compiling

            ICompiler compiler = compilerType.ToLower() == "language"
                ? new AdvancedCompiler(_logger, instructionSet)
                : new SimpleCompiler(instructionSet, _logger);

            _logger.LogInfo("Chose Compiler Type '{0}'", compiler.GetType().Name);

            var result = compiler.Compile(sourceCode);

            if (result == null)
                return;

            _logger.LogSuccess("Done");
            Console.WriteLine();

            // Output
            var output = "";
            foreach (var line in result)
            {
                output += string.Join("", line) + Environment.NewLine;
            }

            _logger.LogInfo("Result:" + Environment.NewLine + output);

            File.WriteAllText(resultFileName, output);

            _logger.LogInfo("Outputted Result into file '{0}'", resultFileName);
        }
    }
}
