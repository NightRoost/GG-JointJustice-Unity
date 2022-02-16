using UnityEngine;

public class Audio : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    private float _masterVolume;

    private void OnValidate()
    {
        AudioListener.volume = _masterVolume;
    }
}
