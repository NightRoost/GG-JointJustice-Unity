using System;
using UnityEngine;

namespace TextDecoder
{
    public class GameplayActionDecoder : MonoBehaviour
    {
        [SerializeField] private NarrativeScriptPlayerComponent _narrativeScriptPlayerComponent;
        [SerializeField] private NarrativeGameState _narrativeGameState;
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }

        public void MODE(string[] parameters)
        {
            _narrativeScriptPlayerComponent.NarrativeScriptPlayer.GameMode = (GameMode)Enum.Parse(typeof(GameMode), parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void LOAD_SCRIPT(string[] parameters)
        {
            _narrativeScriptPlayerComponent.LoadScript(new AssetName(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }

        public void SET_GAME_OVER_SCRIPT(string[] parameters)
        {
            _narrativeGameState.NarrativeScriptStorage.SetGameOverScript(new AssetName(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }

        public void ADD_FAILURE_SCRIPT(string[] parameters)
        {
            _narrativeGameState.NarrativeScriptStorage.AddFailureScript(new AssetName(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }

        public void LOAD_SCENE(string[] parameters)
        {
            _narrativeGameState.SceneLoader.LoadScene(new AssetName(parameters[0]));
        }
    }
}
