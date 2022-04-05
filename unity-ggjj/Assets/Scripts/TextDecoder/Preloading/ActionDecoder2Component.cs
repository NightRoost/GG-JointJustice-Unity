using UnityEngine;
using UnityEngine.Events;

namespace TextDecoding
{
    public class ActionDecoder2Component : MonoBehaviour
    {
        private ActionDecoder2 _actionDecoder;

        [SerializeField] private UnityEvent<int, string> test;
        
        private void Awake()
        {
            _actionDecoder = new ActionDecoder2(this);
            _actionDecoder.Decode("test");
        }
    }
}