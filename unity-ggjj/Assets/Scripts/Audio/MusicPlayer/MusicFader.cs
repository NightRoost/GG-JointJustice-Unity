using UnityEngine;

public class MusicFader
{
    private readonly AudioSource _audioSource;

    private float _normalizedVolume;
    private float NormalizedVolume
    {
        get => _normalizedVolume;
        set
        {
            _normalizedVolume = value;
            _audioSource.volume = value;
        }
    }

    public MusicFader(AudioSource audioModule)
    {
        _audioSource = audioModule;
    }
    
    /// <summary>
    /// To be used in a coroutine to fade in the track
    /// </summary>
    /// <returns>WaitUntil object that returns control to the coroutine when it's finished</returns>
    public WaitUntil FadeIn(float seconds = 1f)
    {
        return new WaitUntil(() =>
        {
            if (seconds <= 0)
            {
                NormalizedVolume = 1f;
                return true;
            }
            
            NormalizedVolume += Time.deltaTime / seconds;

            if (!(NormalizedVolume >= 1f))
            {
                return false;
            }
            
            NormalizedVolume = 1f;
            return true;
        });
    }

    /// <summary>
    /// To be used in a coroutine to fade out the track
    /// </summary>
    /// <returns>WaitUntil object that returns control to the coroutine when it's finished</returns>
    public WaitUntil FadeOut(float seconds = 1f)
    {
        return new WaitUntil(() =>
        {
            if (seconds <= 0)
            {
                NormalizedVolume = 0f;
                return true;
            }

            NormalizedVolume -= Time.deltaTime / seconds;
            
            if (!(NormalizedVolume <= 0))
            {
                return false;
            }
            
            NormalizedVolume = 0;
            return true;
        });
    }
}
