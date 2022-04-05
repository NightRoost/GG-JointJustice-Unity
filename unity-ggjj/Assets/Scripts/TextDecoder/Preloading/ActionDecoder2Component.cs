using UnityEngine;
using UnityEngine.Events;

namespace TextDecoding
{
    public class ActionDecoder2Component : MonoBehaviour
    {
        private ActionDecoder2 _actionDecoder;

        [SerializeField] private UnityEvent<int, string, bool> test;
        
        private void Awake()
        {
            _actionDecoder = new ActionDecoder2(this);
            _actionDecoder.Decode("&test:1,Test,False");
        }

        public void Test(int testInt, string testString, bool testBool)
        {
            Debug.Log(testInt);
            Debug.Log(testString);
            Debug.Log(testBool);
        }
    }
}