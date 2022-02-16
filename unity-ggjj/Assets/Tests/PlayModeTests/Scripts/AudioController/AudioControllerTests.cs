using System.Collections;
using Moq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    // NOTE: As there's no audio hardware present when running headless (i.e. inside automated build processes),
    //       things like ".isPlaying" or ".time"  of AudioSources cannot be asserted, as they will remain
    //       `false` / `0.0f` respectively.
    //       To test this locally, activate `Edit` -> `Project Settings` -> `Audio` -> `Disable Unity Audio`.
    public class AudioControllerTests
    {
        private const string MUSIC_PATH = "Audio/Music/";
        private const string FIRST_SONG = "ABoyAndHisTrial";
        private const string SECOND_SONG = "AKissFromARose";
        private const string THIRD_SONG = "InvestigationJoonyer";

        private MusicPlayer _musicPlayer;
        private AudioModule _audioModule;
        private AudioSource _audioSource;
        private global::DialogueController _dialogueController;

        [UnitySetUp]
        private IEnumerator Setup()
        {
            GameObject audioControllerGameObject = new GameObject("AudioController");
            audioControllerGameObject.AddComponent<AudioListener>(); // required to prevent excessive warnings
            _musicPlayer = audioControllerGameObject.AddComponent<MusicPlayer>();
            _audioModule = audioControllerGameObject.GetComponent<AudioModule>();
            _audioSource = audioControllerGameObject.GetComponent<AudioSource>();
            var dialogueController = audioControllerGameObject.AddComponent<global::DialogueController>();
            TestTools.SetField("_dialogueController", _musicPlayer, dialogueController);
            TestTools.SetField("_activeNarrativeScript", dialogueController, CreateMockNarrativeScript().Object);
            
            // expect error due to missing DirectorActionDecoder
            LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
            LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
            yield return null;
        }

        [UnityTest]
        public IEnumerator AudioControllerPlaySongFadesBetweenSongs()
        {
            var transitionDuration = TestTools.GetField<float>("_transitionDuration", _musicPlayer);

            // setup and verify steady state of music playing for a while
            _musicPlayer.PlaySong(FIRST_SONG);
            yield return new WaitForSeconds(transitionDuration);
            
            Assert.AreEqual(FIRST_SONG, _audioSource.clip.name);
            
            // transition into new song
            _musicPlayer.PlaySong(SECOND_SONG);
            yield return new WaitForSeconds(transitionDuration / 10f);
            
            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(_audioModule.Volume, TestTools.GetField<float>("_maxVolume", _audioModule));
            Assert.AreEqual(FIRST_SONG, _audioSource.clip.name);
            
            yield return new WaitForSeconds(transitionDuration);
            
            // expect new song to be playing at full volume, as we're done transitioning
            // Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(secondSong.name, audioSource.clip.name);
            
            // transition into new song
            _musicPlayer.PlaySong(THIRD_SONG);
            yield return new WaitForSeconds(transitionDuration / 10f);
            
            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            // Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(secondSong.name, audioSource.clip.name);
            
            yield return new WaitForSeconds(transitionDuration);
            
            // expect new song to be playing at full volume, as we're done transitioning
            // Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(thirdSong.name, audioSource.clip.name);
        }

        private Mock<INarrativeScript> CreateMockNarrativeScript()
        {
            var narrativeScriptMock = new Mock<INarrativeScript>();
            narrativeScriptMock.Setup(mock => mock.ObjectStorage.GetObject<AudioClip>(FIRST_SONG)).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}{FIRST_SONG}"));
            narrativeScriptMock.Setup(mock => mock.ObjectStorage.GetObject<AudioClip>(SECOND_SONG)).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}{SECOND_SONG}"));
            narrativeScriptMock.Setup(mock => mock.ObjectStorage.GetObject<AudioClip>(THIRD_SONG)).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}{THIRD_SONG}"));
            return narrativeScriptMock;
        }
    }
}
