namespace SchematicCreator.Parsing
{
    internal class MockParser : IParser
    {
        public bool[,] Parse(string[] content)
        {
            return new bool[,] //Fibonacci
            {
                { true, true, false, false, false, false, true, false,  false, false, false, false, false, false, false, false },
                { true, true, false, false, false, true, false, false,  false, false, false, false, false, false, false, true },
                { true, true, false, false, false, true, true, false,  true, true, true, false, true, false, false, true },
                { false, false, true, false, false, false, false, false,  false, false, true, false, false, true, true, false },
                { true, true, false, true, false, false, false, false,  false, false, false, true, false, false, false, false },
                { false, false, false, true, false, false, true, false,  false, false, true, false, false, true, false, false },
                { false, false, false, true, false, true, false, false,  false, false, true, false, false, true, false, false },
                { true, true, false, true, false, false, false, false,  false, false, false, false, false, false, false, false },
                { true, true, true, true, false, false, false, false,  false, false, false, false, false, false, false, false }
            };
        }
    }
}
