using UnityEngine;

namespace TextDecoder
{
    [RequireComponent(typeof(ActorController))]
    public class ActorActionDecoder : MonoBehaviour
    {
        private ActorController _actorController;
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _actorController = GetComponent<ActorController>();
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }
        
        public void ACTOR(string[] parameters)
        {
            _actorController.SetActiveActor(new AssetName(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }

        public void SHOW_ACTOR(string[] parameters)
        {
            var actorAssetName = parameters.Length > 1 ? new ActorAssetName(parameters[1]) : null;
            _actorController.SetVisibility(true, actorAssetName);
            _actionBroadcaster.OnActionDone();
        }

        public void HIDE_ACTOR(string[] parameters)
        {
            var actorAssetName = parameters.Length > 1 ? new ActorAssetName(parameters[1]) : null;
            _actorController.SetVisibility(false, actorAssetName);
            _actionBroadcaster.OnActionDone();
        }

        public void SPEAK(string[] parameters)
        {
            _actorController.SetActiveSpeaker(new AssetName(parameters[0]), SpeakingType.Speaking);
            _actionBroadcaster.OnActionDone();
        }

        public void THINK(string[] parameters)
        {
            _actorController.SetActiveSpeaker(new AssetName(parameters[0]), SpeakingType.Thinking);
            _actionBroadcaster.OnActionDone();
        }

        public void SPEAK_UNKNOWN(string[] parameters)
        {
            _actorController.SetActiveSpeaker(new AssetName(parameters[0]), SpeakingType.SpeakingWithUnknownName);
            _actionBroadcaster.OnActionDone();
        }

        public void NARRATE(string[] parameters)
        {
            _actorController.SetActiveSpeakerToNarrator();
            _actionBroadcaster.OnActionDone();
        }

        public void SET_POSE(string[] parameters)
        {
            var targetActor = parameters.Length > 1 ? new AssetName(parameters[1]).ToString() : null;
            _actorController.SetPose(new AssetName(parameters[0]), targetActor);
            _actionBroadcaster.OnActionDone();
        }

        public void PLAY_EMOTION(string[] parameters)
        {
            var targetActor = parameters.Length > 1 ? new AssetName(parameters[1]) : null;
            _actorController.PlayEmotion(new AssetName(parameters[0]), targetActor);
        }

        public void SET_ACTOR_POSITION(string[] parameters)
        {
            _actorController.AssignActorToSlot(new AssetName(parameters[0]), new AssetName(parameters[1]));
            _actionBroadcaster.OnActionDone();
        }
    }
}
