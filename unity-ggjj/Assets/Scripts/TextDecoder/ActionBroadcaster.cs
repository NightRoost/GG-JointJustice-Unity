using UnityEngine;

namespace TextDecoder
{
    public class ActionBroadcaster : MonoBehaviour
    {
        [SerializeField] private NarrativeScriptPlayerComponent _narrativeScriptPlayer;
        
        public void BroadcastAction(string actionLine)
        {
            var scriptAction = new ActionParser(actionLine).ScriptAction;
            BroadcastMessage(scriptAction.Name, scriptAction.Parameters, SendMessageOptions.RequireReceiver);
        }

        public void OnActionDone()
        {
            _narrativeScriptPlayer.Continue();
        }
    }
}
