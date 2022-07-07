using UnityEngine;

namespace TextDecoder
{
    public class EvidenceActionDecoder : MonoBehaviour
    {
        [SerializeField] private NarrativeGameState _narrativeGameState;
        
        private EvidenceController _evidenceController;
        private ActionBroadcaster _actionBroadcaster;

        private IObjectStorage ObjectStorage => _narrativeGameState.ObjectStorage;
        
        private void Awake()
        {
            _evidenceController = GetComponent<EvidenceController>();
            _actionBroadcaster = transform.parent.GetComponent<ActionBroadcaster>();
        }

        public void ADD_EVIDENCE(string[] parameters)
        {
            _evidenceController.AddEvidence(ObjectStorage.GetObject<EvidenceData>(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }
        
        public void REMOVE_EVIDENCE(string[] parameters)
        {
            _evidenceController.RemoveEvidence(ObjectStorage.GetObject<EvidenceData>(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }
        
        public void ADD_RECORD(string[] parameters)
        {
            _evidenceController.AddRecord(ObjectStorage.GetObject<ActorData>(parameters[0]));
            _actionBroadcaster.OnActionDone();
        }

        public void PRESENT_EVIDENCE()
        {
            _evidenceController.RequirePresentEvidence();
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.GameMode = GameMode.CrossExamination;
        }
        
        public void SUBSTITUTE_EVIDENCE(string[] parameters)
        {
            _evidenceController.SubstituteEvidence(ObjectStorage.GetObject<EvidenceData>(parameters[0]), ObjectStorage.GetObject<EvidenceData>(parameters[1]));
            _actionBroadcaster.OnActionDone();
        }
    }
}
