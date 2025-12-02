using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public SoundType soundType;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 1f)] public float volumeVariation = 0f;

    [Header("Pitch Settings")]
    [Range(0.1f, 3f)] public float pitch = 1f;
    [Range(0f, 1f)] public float pitchVariation = 0f;

    [Header("Playback Settings")]
    public bool loop = false;
    public bool playOnAwake = false;
    [Range(0f, 1f)] public float spatialBlend = 0f;

    [Header("3D Sound Settings")]
    public float minDistance = 1f;
    public float maxDistance = 500f;
}
