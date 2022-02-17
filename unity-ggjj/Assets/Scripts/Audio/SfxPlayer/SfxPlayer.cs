using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour, ISfxPlayer
{
    [SerializeField]
    private DirectorActionDecoder _directorActionDecoder;

    [SerializeField]
    private DialogueController _dialogueController;
    
    private AudioSource _AudioSource;
    
    private void Awake()
    {
        _directorActionDecoder.Decoder.SfxPlayer = this;
        _AudioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySfx(AudioClip audioClip)
    {
        _AudioSource.PlayOneShot(audioClip);
    }
}
