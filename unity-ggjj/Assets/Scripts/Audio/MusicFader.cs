using UnityEngine;

public class MusicFader
{
    private readonly AudioModule _audioModule;

    public MusicFader(AudioModule audioModule)
    {
        _audioModule = audioModule;
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
                _audioModule.Volume = 1f;
                return true;
            }

            _audioModule.Volume += Time.deltaTime / seconds;

            if (!(_audioModule.Volume >= 1f))
            {
                return false;
            }
            
            _audioModule.Volume = 1f;
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
                _audioModule.Volume = 0f;
                return true;
            }

            _audioModule.Volume -= Time.deltaTime / seconds;

            if (!(_audioModule.Volume <= 0))
            {
                return false;
            }
            
            _audioModule.Volume = 0;
            return true;
        });
    }
}
