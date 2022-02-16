using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioModule))]
public class MusicPlayer : MonoBehaviour, IAudioController
{
    [Tooltip("Drag a DialogueController here")]
    [SerializeField] private DialogueController _dialogueController;
    
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;

    [Tooltip("Total duration of fade out + fade in")]
    [Range(0f, 4f)]
    [SerializeField] private float _transitionDuration = 2f;
    
    private AudioModule _audioModule;

    private bool IsCurrentlyPlayingMusic => _audioModule.IsPlaying && _audioModule.Volume != 0;
    
    private void Awake()
    {
        _audioModule = GetComponent<AudioModule>();
        _audioModule.ShouldLoop = true;
    }
    
    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Start()
    {
        _directorActionDecoder.Decoder.AudioController = this;
    }

    /// <summary>
    /// Fades in the song with the desired name. Cancels out of a current fade if one is in progress.
    /// </summary>
    /// <param name="songName">Name of song asset, must be in `Resources/Audio/Music`</param>
    public void PlaySong(string songName)
    {
        var song = _dialogueController.ActiveNarrativeScript.ObjectStorage.GetObject<AudioClip>(songName);
        StartCoroutine(FadeToNewSong(song));
    }

    /// <summary>
    /// Plays sound effect of desired name.
    /// </summary>
    /// <param name="soundEffectName">Name of sound effect asset, must be in `Resources/Audio/SFX`</param>
    public void PlaySfx(string soundEffectName)
    {
        var soundEffectClip = _dialogueController.ActiveNarrativeScript.ObjectStorage.GetObject<AudioClip>(soundEffectName);
        _audioModule.PlayOneShot(soundEffectClip);
    }

    /// <summary>
    /// Coroutine to fade to a new song.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    public IEnumerator FadeToNewSong(AudioClip song)
    {
        var musicFader = new MusicFader(_audioModule);
        if (IsCurrentlyPlayingMusic)
        {
            yield return musicFader.FadeOut(_transitionDuration / 2f);
        }

        SetCurrentTrack(song);
        yield return musicFader.FadeIn(_transitionDuration / 2f);
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    public void StopSong()
    {
        _audioModule.Stop();
    }

    /// <summary>
    /// Assigns the music player to a given song with 0 volume, intending to fade it in.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    private void SetCurrentTrack(AudioClip song)
    {
        _audioModule.Volume = 0f;
        _audioModule.Play(song);
    }
}
