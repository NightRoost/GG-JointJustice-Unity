using UnityEngine;

namespace TextDecoder
{
    public class AppearingDialogueActionDecoder : MonoBehaviour
    {
        private AppearingDialogueController _appearingDialogueController;
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _appearingDialogueController = GetComponent<AppearingDialogueController>();
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }

        public void DIALOGUE_SPEED(string[] parameters)
        {
            _appearingDialogueController.CharacterDelay = float.Parse(parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void PUNCTUATION_SPEED(string[] parameters)
        {
            _appearingDialogueController.DefaultPunctuationDelay = float.Parse(parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void AUTO_SKIP(string[] parameters)
        {
            _appearingDialogueController.AutoSkip = bool.Parse(parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void DISABLE_SKIPPING(string[] parameters)
        {
            _appearingDialogueController.SkippingDisabled = bool.Parse(parameters[0]);
            _actionBroadcaster.OnActionDone();
        }

        public void CONTINUE_DIALOGUE()
        {
            _appearingDialogueController.ContinueDialogue = true;
            _actionBroadcaster.OnActionDone();
        }

        public void APPEAR_INSTANTLY()
        {
            _appearingDialogueController.AppearInstantly = true;
            _actionBroadcaster.OnActionDone();
        }

        public void HIDE_TEXTBOX()
        {
            _appearingDialogueController.TextBoxHidden = true;
            _actionBroadcaster.OnActionDone();
        }
    }
}
