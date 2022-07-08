using UnityEngine;

namespace TextDecoder
{
    public class ActionBroadcaster : MonoBehaviour, IActionBroadcaster
    {
        [SerializeField] private NarrativeScriptPlayerComponent _narrativeScriptPlayer;
        
        public void BroadcastAction(string actionLine)
        {
            var scriptAction = new ScriptAction(actionLine);
            BroadcastMessage(scriptAction.Name, scriptAction.Parameters, SendMessageOptions.RequireReceiver);
        }

        public void OnActionDone()
        {
            _narrativeScriptPlayer.Continue();
        }
    }
}
