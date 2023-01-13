namespace MCCompiler.Assembler.TargetCodeGenerator;

internal interface ITargetTranslator
{
    string[] Translate(List<IntermediateInstruction> instructions);
}