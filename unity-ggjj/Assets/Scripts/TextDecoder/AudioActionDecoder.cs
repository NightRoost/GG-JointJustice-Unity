using UnityEngine;

namespace TextDecoder
{
    public class AudioActionDecoder : MonoBehaviour
    {
        [SerializeField] private NarrativeGameState _narrativeGameState;
        
        private AudioController _audioController;
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _audioController = GetComponent<AudioController>();
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }

        public void PLAY_SFX(string[] parameters)
        {
            _audioController.PlaySfx(_narrativeGameState.ObjectStorage.GetObject<AudioClip>(new AssetName(parameters[0])));
            _actionBroadcaster.OnActionDone();
        }

        public void PLAY_SONG(string[] parameters)
        {
            var transitionTime = parameters.Length > 1 ? float.Parse(parameters[1]) : 0f;
            _audioController.PlaySong(_narrativeGameState.ObjectStorage.GetObject<AudioClip>(new AssetName(parameters[0])), transitionTime);
            _actionBroadcaster.OnActionDone();
        }

        public void STOP_SONG()
        {
            _audioController.StopSong();
            _actionBroadcaster.OnActionDone();
        }

        public void FADE_OUT_SONG(string[] parameters)
        {
            _audioController.FadeOutSong(float.Parse(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }
    }
}
