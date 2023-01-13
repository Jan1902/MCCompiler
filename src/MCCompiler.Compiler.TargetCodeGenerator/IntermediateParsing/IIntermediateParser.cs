namespace MCCompiler.Assembler.TargetCodeGenerator.IntermediateParsing;

internal interface IIntermediateParser
{
    List<IntermediateInstruction> Parse(string[] input);
}