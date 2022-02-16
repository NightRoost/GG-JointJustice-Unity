using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioModule : MonoBehaviour
{
    private AudioSource _audioSource;

    public float Volume
    {
        get => _audioSource.volume;
        set => _audioSource.volume = value;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
}
