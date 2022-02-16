public interface IDialogueController
{
    GameMode GameMode { set; }
    NarrativeScript ActiveNarrativeScript { get; }
    
    void StartSubStory(NarrativeScript narrativeScript);
}