using UnityEngine;

[CreateAssetMenu(menuName = "Sound effect")]
public class SoundEffect : ScriptableObject
{
    [Header("Pitch")]
    [Tooltip("Uses MinPitch if false")] public bool HasRandomizedPitch;
    [Range(0f, 2f)] public float MinPitch = 1f;
    [Range(0f, 2f)] public float MaxPitch = 1f;

    [Header("Volume")]
    [Tooltip("Uses MinVolume if false")] public bool HasRandomizedVolume;
    [Range(0f, 1f)] public float MinVolume = 0.5f;
    [Range(0f, 1f)] public float MaxVolume = 0.5f;

    [Header("Sounds")]
    [Tooltip("Uses AudioClips[0] if false")] public bool HasRandomizedClips;
    public AudioClip[] AudioClips;
}
