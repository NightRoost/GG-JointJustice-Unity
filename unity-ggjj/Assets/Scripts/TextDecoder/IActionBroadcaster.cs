namespace TextDecoder
{
    public interface IActionBroadcaster
    {
        void BroadcastAction(string actionLine);
        void OnActionDone();
    }
}