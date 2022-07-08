using UnityEngine;

namespace TextDecoder
{
    public interface IActionBroadcaster
    {
        void BroadcastAction(string actionLine, SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver);
        void OnActionDone();
    }
}