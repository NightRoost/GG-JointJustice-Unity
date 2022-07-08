using System;
using UnityEngine;

namespace TextDecoder
{
    public class SceneActionDecoder : MonoBehaviour
    {
        [SerializeField] private NarrativeGameState _narrativeGameState;
        
        private SceneController _sceneController;
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _sceneController = GetComponent<SceneController>();
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }
        
        public void FADE_OUT(string[] parameters)
        {
            _sceneController.FadeOut(float.Parse(parameters[0]));
        }
        
        public void FADE_IN(string[] parameters)
        {
            _sceneController.FadeIn(float.Parse(parameters[0]));
        }

        public void CAMERA_PAN(string[] parameters)
        {
            _sceneController.PanCamera(float.Parse(parameters[0]), new Vector2Int(int.Parse(parameters[1]), int.Parse(parameters[2])));
            _actionBroadcaster.OnActionDone();
        }

        public void CAMERA_SET(string[] parameters)
        {
            _sceneController.SetCameraPos(new Vector2Int(int.Parse(parameters[0]), int.Parse(parameters[1])));
            _actionBroadcaster.OnActionDone();
        }

        public void SHAKE_SCREEN(string[] parameters)
        {
            var isBlocking = parameters.Length > 2 && bool.Parse(parameters[3]);
            _sceneController.ShakeScreen(float.Parse(parameters[0]), float.Parse(parameters[1]), isBlocking);
        }

        public void SCENE(string[] parameters)
        {
            _sceneController.SetScene(parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void SHOW_ITEM(string[] parameters)
        {
            _sceneController.ShowItem(_narrativeGameState.ObjectStorage.GetObject<ICourtRecordObject>(parameters[0]), (ItemDisplayPosition)Enum.Parse(typeof(ItemDisplayPosition), parameters[1]));
            _actionBroadcaster.OnActionDone();
        }

        public void HIDE_ITEM()
        {
            _sceneController.HideItem();
            _actionBroadcaster.OnActionDone();
        }

        public void PLAY_ANIMATION(string[] parameters)
        {
            _sceneController.PlayAnimation(parameters[0]);
        }

        public void JUMP_TO_POSITION(string[] parameters)
        {
            _sceneController.JumpToActorSlot(parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void PAN_TO_POSITION(string[] parameters)
        {
            _sceneController.PanToActorSlot(parameters[0], float.Parse(parameters[1]));
            _actionBroadcaster.OnActionDone();
        }

        public void RELOAD_SCENE()
        {
            _sceneController.ReloadScene();
        }

        public void WAIT(string[] parameters)
        {
            _sceneController.Wait(float.Parse(parameters[0]));
        }

        public void OBJECTION(string[] parameters)
        {
            _sceneController.Shout(new AssetName(parameters[0]), "Objection", true);
        }

        public void TAKE_THAT(string[] parameters)
        {
            _sceneController.Shout(new AssetName(parameters[0]), "TakeThat", true);
        }

        public void HOLD_IT(string[] parameters)
        {
            _sceneController.Shout(new AssetName(parameters[0]), "HoldIt", true);
        }

        public void SHOUT(string[] parameters)
        {
            var allowRandomShouts = parameters.Length > 2 && bool.Parse(parameters[3]);
            _sceneController.Shout(new AssetName(parameters[0]), parameters[1], allowRandomShouts);
        }

        public void BEGIN_WITNESS_TESTIMONY()
        {
            _sceneController.WitnessTestimonyActive = true;
            _actionBroadcaster.OnActionDone();
        }

        public void END_WITNESS_TESTIMONY()
        {
            _sceneController.WitnessTestimonyActive = false;
            _actionBroadcaster.OnActionDone();
        }
    }
}
