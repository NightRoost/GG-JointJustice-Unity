using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour, IMusicPlayer
{
    [Tooltip("Drag a DialogueController here")]
    [SerializeField] private DialogueController _dialogueController;
    
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;

    [Tooltip("Total duration of fade out + fade in")]
    [Range(0f, 4f)]
    [SerializeField] private float _transitionDuration = 2f;
    
    private AudioSource _audioSource;
    private MusicFader _musicFader;

    private bool IsCurrentlyPlayingMusic => _audioSource.isPlaying && _audioSource.volume != 0;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _musicFader = new MusicFader(_audioSource);
    }
    
    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Start()
    {
        _directorActionDecoder.Decoder.MusicPlayer = this;
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
    /// Coroutine to fade to a new song.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    public IEnumerator FadeToNewSong(AudioClip song)
    {
        if (IsCurrentlyPlayingMusic)
        {
            yield return _musicFader.FadeOut(_transitionDuration / 2f);
        }

        SetCurrentTrack(song);
        yield return _musicFader.FadeIn(_transitionDuration / 2f);
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    public void StopSong()
    {
        _audioSource.Stop();
    }

    /// <summary>
    /// Assigns the music player to a given song with 0 volume, intending to fade it in.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    private void SetCurrentTrack(AudioClip song)
    {
        _audioSource.volume = 0f;
        _audioSource.clip = song;
        _audioSource.Play();
    }
}
