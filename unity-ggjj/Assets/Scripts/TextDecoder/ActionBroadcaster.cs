using UnityEngine;
using UnityEngine.Events;

namespace TextDecoder
{
    public class ActionBroadcaster : MonoBehaviour, IActionBroadcaster
    {
        [SerializeField] private UnityEvent _onActionDone;
        
        public void BroadcastAction(string actionLine, SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver)
        {
            var scriptAction = new ScriptAction(actionLine);
            BroadcastMessage(scriptAction.Name, scriptAction.Parameters, SendMessageOptions.RequireReceiver);
        }

        public void OnActionDone()
        {
            _onActionDone.Invoke();
        }
    }
}
