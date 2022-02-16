using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioModule : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float _maxVolume = 1;
    
    private AudioSource _audioSource;

    public bool IsPlaying => _audioSource.isPlaying;
    public bool ShouldLoop
    {
        get => _audioSource.loop;
        set => _audioSource.loop = value;
    }
    public float Volume
    {
        get => _audioSource.volume;
        set => _audioSource.volume = value * _maxVolume;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        _audioSource.volume = _maxVolume;
    }

    public void Play(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void PlayWithoutInterrupting(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
        {
            return;
        }

        Play(audioClip);
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
}
