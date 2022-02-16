using System;
using System.Collections;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.AudioController
{
    // NOTE: As there's no audio hardware present when running headless (i.e. inside automated build processes),
    //       things like ".isPlaying" or ".time"  of AudioSources cannot be asserted, as they will remain
    //       `false` / `0.0f` respectively.
    //       To test this locally, activate `Edit` -> `Project Settings` -> `Audio` -> `Disable Unity Audio`.
    public class AudioControllerTests
    {
        private const string MUSIC_PATH = "Audio/Music/";

        [UnityTest]
        public IEnumerator AudioController_PlaySong_FadesBetweenSongs()
        {
            GameObject audioControllerGameObject = new GameObject("AudioController");
            audioControllerGameObject.AddComponent<AudioListener>(); // required to prevent excessive warnings
            var musicPlayer = audioControllerGameObject.AddComponent<MusicPlayer>();
            var audioModule = audioControllerGameObject.GetComponent<AudioModule>();

            // expect error due to missing DirectorActionDecoder
            LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
            yield return null;
            
            var type = musicPlayer.GetType().GetField("_transitionDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(type);
            float transitionDuration = (float)type.GetValue(musicPlayer);

            var dialogueControllerMock = CreateDialogueControllerMock();
            TestTools.SetField("_dialogueController", dialogueControllerMock.Object, musicPlayer);

            // setup and verify steady state of music playing for a while
            musicPlayer.PlaySong("ABoyAndHisTrial");
            yield return new WaitForSeconds(transitionDuration);
            yield return new WaitForSeconds(50);
            // Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(firstSong.name, audioSource.clip.name);
            //
            // // transition into new song
            // var secondSong = Resources.Load<AudioClip>($"{MUSIC_PATH}aKissFromARose");
            // audioModule.Play(secondSong); // TODO FIX
            // yield return new WaitForSeconds(transitionDuration/10f);
            //
            // // expect old song to still be playing, but no longer at full volume, as we're transitioning
            // Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(firstSong.name, audioSource.clip.name);
            //
            // yield return new WaitForSeconds(transitionDuration);
            //
            // // expect new song to be playing at full volume, as we're done transitioning
            // Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(secondSong.name, audioSource.clip.name);
            //
            // // transition into new song
            // var thirdSong = Resources.Load<AudioClip>($"{MUSIC_PATH}investigationJoonyer");
            // audioModule.Play(thirdSong); // TODO FIX
            // yield return new WaitForSeconds(transitionDuration/10f);
            //
            // // expect old song to still be playing, but no longer at full volume, as we're transitioning
            // Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(secondSong.name, audioSource.clip.name);
            //
            // yield return new WaitForSeconds(transitionDuration);
            //
            // // expect new song to be playing at full volume, as we're done transitioning
            // Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            // Assert.AreEqual(thirdSong.name, audioSource.clip.name);
        }

        private Mock<IDialogueController> CreateDialogueControllerMock()
        {
            var objectStorageMock = new Mock<IObjectStorage>();
            var dialogueControllerMock = new Mock<IDialogueController>();
            
            objectStorageMock.Setup(mock => mock.GetObject<AudioClip>("ABoyAndHisTrial")).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}ABoyAndHisTrial"));
            objectStorageMock.Setup(mock => mock.GetObject<AudioClip>("AKissFromARose")).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}AKissFromARose"));
            objectStorageMock.Setup(mock => mock.GetObject<AudioClip>("InvestigationJoonyer")).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}InvestigationJoonyer"));
            
            TestTools.SetField("_objectStorage", dialogueControllerMock.Object, objectStorageMock.Object);
            
            dialogueControllerMock.Setup(mock => mock.ActiveNarrativeScript.ObjectStorage.GetObject<AudioClip>("ABoyAndHisTrial")).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}ABoyAndHisTrial"));
            dialogueControllerMock.Setup(mock => mock.ActiveNarrativeScript.ObjectStorage.GetObject<AudioClip>("AKissFromARose")).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}AKissFromARose"));
            dialogueControllerMock.Setup(mock => mock.ActiveNarrativeScript.ObjectStorage.GetObject<AudioClip>("InvestigationJoonyer")).Returns(Resources.Load<AudioClip>($"{MUSIC_PATH}InvestigationJoonyer"));
            return dialogueControllerMock;
        }
    }
}
