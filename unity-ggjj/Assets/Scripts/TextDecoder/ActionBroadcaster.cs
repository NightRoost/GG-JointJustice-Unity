using UnityEngine;

namespace TextDecoder
{
    public class ActionBroadcaster : MonoBehaviour
    {
        public void BroadcastAction(string actionLine)
        {
            var scriptAction = new ActionParser(actionLine).ScriptAction;
            BroadcastMessage(scriptAction.Name, scriptAction.Parameters);
        }
    }
}
