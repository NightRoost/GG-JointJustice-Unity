using SaveFiles;
using UnityEngine;

namespace TextDecoder
{
    public class SaveDataActionDecoder : MonoBehaviour
    {
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }
    
        private void UNLOCK_CHAPTER(SaveData.Progression.Chapters chapter)
        {
            PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData data) => {
                data.GameProgression.UnlockedChapters |= chapter;
            });
            _actionBroadcaster.OnActionDone();
        }
    }
}
