namespace SchematicCreator.Parsing
{
    internal interface IParser
    {
        bool[,] Parse(string[] content);
    }
}