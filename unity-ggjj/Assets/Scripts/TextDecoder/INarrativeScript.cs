using Ink.Runtime;

public interface INarrativeScript
{
    IObjectStorage ObjectStorage { get; }
    Story Story { get; }
    string Name { get; }
}