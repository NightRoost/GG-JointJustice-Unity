using UnityEngine;

[RequireComponent(typeof(AudioModule))]
public class SfxPlayer : MonoBehaviour, ISfxPlayer
{
    [SerializeField]
    private DirectorActionDecoder _directorActionDecoder;

    [SerializeField]
    private DialogueController _dialogueController;
    
    private AudioModule _audioModule;
    
    private void Awake()
    {
        _directorActionDecoder.Decoder.SfxPlayer = this;
        _audioModule = GetComponent<AudioModule>();
    }
    
    public void PlaySfx(string audioClipName)
    {
        var audioClip = _dialogueController.ActiveNarrativeScript.ObjectStorage.GetObject<AudioClip>(audioClipName);
        _audioModule.PlayOneShot(audioClip);
    }
}
