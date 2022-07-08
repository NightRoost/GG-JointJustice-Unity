using System;
using UnityEngine;

namespace TextDecoder
{
    public class PenaltyActionDecoder : MonoBehaviour
    {
        private PenaltyManager _penaltyManager;
        private ActionBroadcaster _actionBroadcaster;

        private void Awake()
        {
            _penaltyManager = GetComponent<PenaltyManager>();
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }

        public void RESET_PENALTIES()
        {
            _penaltyManager.ResetPenalties();
            _actionBroadcaster.OnActionDone();
        }

        public void MODE(string[] parameters)
        {
            var mode = (GameMode)Enum.Parse(typeof(GameMode), parameters[0]);
            switch (mode)
            {
                case GameMode.Dialogue:
                    _penaltyManager.OnCrossExaminationEnd();
                    break;
                case GameMode.CrossExamination:
                    _penaltyManager.OnCrossExaminationStart();
                    break;
                default:
                    throw new NotSupportedException($"Switching to game mode '{mode}' is not supported");
            }
        }

        public void ISSUE_PENALTY()
        {
            _penaltyManager.Decrement();
            _actionBroadcaster.OnActionDone();
        }
    }
}
