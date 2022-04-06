using UnityEngine;
using UnityEngine.Events;

namespace TextDecoding
{
    public class DialogueActionDecoder : ActionDecoder2Component
    {
        [SerializeField] public UnityEvent<string> test2;

        private void Awake()
        {
            Debug.Log("Test");
        }
    }
}