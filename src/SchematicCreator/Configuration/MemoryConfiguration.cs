using System.Text.Json.Serialization;

namespace SchematicCreator.Configuration;

internal class MemoryConfiguration
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MemoryType Type { get; set; }
    public int SpacingX { get; set; }
    public int SpacingY { get; set; }
    public int SpacingZ { get; set; }
    public int CellsPerBlockOrPage { get; set; }
    public int BlockOrPageCount { get; set; }
    public bool AlternatingHeight { get; set; }
    public int InstructionSize { get; set; }
    public string OnMaterial { get; set; }
    public string OffMaterial { get; set; }
}
