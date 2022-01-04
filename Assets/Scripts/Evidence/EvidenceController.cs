using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [Tooltip("Drag the DialogueController here")]
    [SerializeField] private DialogueController _dialogueController;
    
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when the PRESENT_EVIDENCE action is called.")]
    [FormerlySerializedAs("_onOpenEvidenceMenu")]
    [SerializeField] private UnityEvent _onRequirePresentEvidence;

    [Tooltip("This event is called when a piece of evidence is clicked")]
    [SerializeField] private UnityEvent<ICourtRecordObject> _onPresentEvidence;

    [Tooltip("Drag an EvidenceMenu component here, which will updated when the game state (i.e. ability to present evidence) changes.")]
    [SerializeField] public EvidenceMenu _evidenceMenu;

    public List<Evidence> CurrentEvidence { get; } = new List<Evidence>();
    public List<ActorData> CurrentProfiles { get; } = new List<ActorData>();

    /// <summary>
    /// Called either when invoking <see cref="DialogueController._onCrossExaminationLoopActive" />
    /// or when a `PRESENT_EVIDENCE`-action has been encountered
    /// </summary>
    /// <param name="canPresentEvidence">Set to true, if evidence can be presented, set to false if presenting evidence is currently disabled</param>
    public void SetCanPresentEvidence(bool canPresentEvidence)
    {
        _evidenceMenu.CanPresentEvidence = canPresentEvidence;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Evidence Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.Decoder.EvidenceController = this;
        }
    }

    /// <summary>
    /// Adds a piece of evidence to the evidence menu. Gets an Evidence object
    /// from _masterEvidenceDictionary and adds it to _currentEvidenceDictionary
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to add.</param>
    public void AddEvidence(string evidenceName)
    {
        CurrentEvidence.Add(_dialogueController.NarrativeScript.ObjectStorage.GetObject<Evidence>(evidenceName));
    }

    /// <summary>
    /// Removes a piece of evidence from the evidence menu.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidenceName)
    {
        CurrentEvidence.Remove(_dialogueController.NarrativeScript.ObjectStorage.GetObject<Evidence>(evidenceName));
    }

    /// <summary>
    /// Adds an actor to the court record.
    /// </summary>
    /// <param name="actorName">The name of the actor to add.</param>
    public void AddToCourtRecord(string actorName)
    {
        CurrentProfiles.Add(_dialogueController.NarrativeScript.ObjectStorage.GetObject<ActorData>(actorName));
    }

    /// <summary>
    /// Method called by DirectorActionDecoder to open the evidence menu and require the user to present a piece of evidence.
    /// Calls an event which should open (and disable closing of) the evidence menu.
    /// </summary>
    public void RequirePresentEvidence()
    {
        SetCanPresentEvidence(true);
        _onRequirePresentEvidence.Invoke();
    }

    /// <summary>
    /// Substitutes a piece of evidence with its assigned alternate evidence.
    /// </summary>
    /// <param name="evidenceName"></param>
    public void SubstituteEvidenceWithAlt(string evidenceName)
    {
        int evidenceIndex = CurrentEvidence.IndexOf(_dialogueController.NarrativeScript.ObjectStorage.GetObject<Evidence>(evidenceName));
        CurrentEvidence[evidenceIndex] = CurrentEvidence[evidenceIndex].AltEvidence;
    }

    /// <summary>
    /// This method is called by the EvidenceMenu when evidence has been
    /// clicked and needs to be presented.
    /// </summary>
    /// <param name="evidence">The evidence to present.</param>
    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
        _onPresentEvidence.Invoke(evidence);
    }
}
