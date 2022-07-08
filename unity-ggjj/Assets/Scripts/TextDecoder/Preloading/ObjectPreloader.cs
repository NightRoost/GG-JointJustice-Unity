using System;
using TextDecoder;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Assigned to an ActionDecoder in place of the usual controller interfaces.
/// Action decoder calls the methods which are used to load any required objects.
/// Objects are then stored in the assign ObjectStorage object.
/// </summary>
public class ObjectPreloader : MonoBehaviour
{
    [SerializeField] private NarrativeGameState _narrativeGameState;
    
    protected void ADD_EVIDENCE(string[] parameters)
    {
        LoadEvidence(new AssetName(parameters[0]));
    }

    protected void ADD_RECORD(string[] parameters)
    {
        LoadActor(new AssetName(parameters[0]));
    }

    protected  void PLAY_SFX(string[] parameters)
    {
        LoadObject<AudioClip>($"Audio/SFX/{new AssetName(parameters[0])}");
    }

    protected  void PLAY_SONG(string[] parameters)
    {
        LoadObject<AudioClip>($"Audio/Music/{new AssetName(parameters[0])}");
    }

    protected  void SCENE(string[] parameters)
    {
        LoadObject<BGScene>($"BGScenes/{new AssetName(parameters[0])}");
    }

    protected  void SHOW_ITEM(string[] parameters)
    {
        LoadEvidence(new AssetName(parameters[0]));
    }

    protected  void ACTOR(string[] parameters)
    {
        LoadActor(new AssetName(parameters[0]));
    }

    protected  void SPEAK(string[] parameters)
    {
        LoadActor(new AssetName(parameters[0]));
    }

    protected  void SPEAK_UNKNOWN(string[] parameters)
    {
        LoadActor(new AssetName(parameters[0]));
    }

    protected  void THINK(string[] parameters)
    {
        LoadActor(new AssetName(parameters[0]));
    }

    protected  void SET_ACTOR_POSITION(string[] parameters)
    {
        LoadActor(new AssetName(parameters[1]));
    }

    private void LoadActor(string actorName)
    {
        LoadObject<ActorData>($"Actors/{actorName}");
    }
    
    private void LoadEvidence(string evidenceName)
    {
        LoadObject<EvidenceData>($"Evidence/{evidenceName}");
    }

    /// <summary>
    /// Loads an object and adds it to the object storage
    /// </summary>
    /// <param name="path">The path to the object to load</param>
    private void LoadObject<T>(string path) where T : Object
    {
        try
        {
            var obj = Resources.Load<T>(path);
            if (!_narrativeGameState.ObjectStorage.Contains(obj))
            {
                _narrativeGameState.ObjectStorage.Add(obj);
            }
        }
        catch (NullReferenceException exception)
        {
            throw new ObjectLoadingException($"{exception.GetType().Name}: Object at path {path} could not be loaded", exception);
        }
    }
}