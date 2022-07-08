using System;
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
    
        private void UNLOCK_CHAPTER(string[] parameters)
        {
            PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData data) => {
                data.GameProgression.UnlockedChapters |= (SaveData.Progression.Chapters)Enum.Parse(typeof(SaveData.Progression.Chapters), parameters[0]);
            });
            _actionBroadcaster.OnActionDone();
        }
    }
}
