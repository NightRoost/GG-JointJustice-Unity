public interface IDialogueController
{
    GameMode GameMode { set; }
    INarrativeScript ActiveNarrativeScript { get; }
    
    void StartSubStory(NarrativeScript narrativeScript);
}