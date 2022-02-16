using System.Collections;
using System.Reflection;
using NUnit.Framework;
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
            var audioModule = musicPlayer.GetComponent<AudioModule>();

            // expect error due to missing DirectorActionDecoder
            LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
            yield return null;
            AudioSource audioSource = audioControllerGameObject.transform.Find("Music Player").GetComponent<AudioSource>();

            FieldInfo type = musicPlayer.GetType().GetField("_transitionDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            if (type is null) // needed to satisfy Intellisense's "possible NullReferenceException" in line below conditional
            {
                Assert.IsNotNull(type);
            }
            float transitionDuration = (float)type.GetValue(musicPlayer);

            FieldInfo settingsMusicVolumeType = musicPlayer.GetType().GetField("_settingsMusicVolume", BindingFlags.NonPublic | BindingFlags.Instance);
            if (settingsMusicVolumeType is null) // needed to satisfy Intellisense's "possible NullReferenceException" in line below conditional
            {
                Assert.IsNotNull(settingsMusicVolumeType);
            }
            float settingsMusicVolume = (float)settingsMusicVolumeType.GetValue(musicPlayer);

            // setup and verify steady state of music playing for a while
            var firstSong = Resources.Load<AudioClip>($"{MUSIC_PATH}aBoyAndHisTrial");
            audioModule.Play(firstSong); // TODO FIX
            yield return new WaitForSeconds(transitionDuration);

            Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(firstSong.name, audioSource.clip.name);

            // transition into new song
            var secondSong = Resources.Load<AudioClip>($"{MUSIC_PATH}aKissFromARose");
            audioModule.Play(secondSong); // TODO FIX
            yield return new WaitForSeconds(transitionDuration/10f);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(firstSong.name, audioSource.clip.name);

            yield return new WaitForSeconds(transitionDuration);

            // expect new song to be playing at full volume, as we're done transitioning
            Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(secondSong.name, audioSource.clip.name);

            // transition into new song
            var thirdSong = Resources.Load<AudioClip>($"{MUSIC_PATH}investigationJoonyer");
            audioModule.Play(thirdSong); // TODO FIX
            yield return new WaitForSeconds(transitionDuration/10f);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(secondSong.name, audioSource.clip.name);

            yield return new WaitForSeconds(transitionDuration);

            // expect new song to be playing at full volume, as we're done transitioning
            Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(thirdSong.name, audioSource.clip.name);
        }
    }
}
