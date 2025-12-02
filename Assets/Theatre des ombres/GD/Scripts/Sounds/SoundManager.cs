using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private int audioSourcePoolSize = 10;

    [Header("Mixer Groups (Optional)")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup uiMixerGroup;
    [SerializeField] private AudioMixerGroup ambianceMixerGroup;
    [SerializeField] private AudioMixerGroup voiceMixerGroup;

    [Header("Sound Library")]
    [SerializeField] private List<SoundData> soundLibrary = new List<SoundData>();

    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();
    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();
    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSoundLibrary();
        InitializeAudioSourcePool();
    }

    private void InitializeSoundLibrary()
    {
        soundDictionary.Clear();
        foreach (SoundData sound in soundLibrary)
        {
            if (sound != null && !soundDictionary.ContainsKey(sound.soundName))
            {
                soundDictionary.Add(sound.soundName, sound);
            }
        }
    }

    private void InitializeAudioSourcePool()
    {
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            CreateAudioSource();
        }
    }

    private AudioSource CreateAudioSource()
    {
        GameObject audioSourceObject = new GameObject($"AudioSource_{audioSourcePool.Count}");
        audioSourceObject.transform.SetParent(transform);
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSourcePool.Enqueue(audioSource);
        return audioSource;
    }

    private AudioSource GetAudioSource()
    {
        if (audioSourcePool.Count == 0)
        {
            return CreateAudioSource();
        }

        AudioSource source = audioSourcePool.Dequeue();
        activeAudioSources.Add(source);
        return source;
    }

    private void ReturnAudioSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        activeAudioSources.Remove(source);
        audioSourcePool.Enqueue(source);
    }

    public void PlaySound(string soundName, Vector3 position = default)
    {
        if (!soundDictionary.TryGetValue(soundName, out SoundData soundData))
        {
            Debug.LogWarning($"Sound '{soundName}' not found in library!");
            return;
        }

        AudioSource source = GetAudioSource();
        ConfigureAudioSource(source, soundData, position);
        source.Play();

        if (!soundData.loop)
        {
            StartCoroutine(ReturnAudioSourceAfterPlay(source, soundData.clip.length));
        }
    }

    public AudioSource PlaySoundWithReference(string soundName, Vector3 position = default)
    {
        if (!soundDictionary.TryGetValue(soundName, out SoundData soundData))
        {
            Debug.LogWarning($"Sound '{soundName}' not found in library!");
            return null;
        }

        AudioSource source = GetAudioSource();
        ConfigureAudioSource(source, soundData, position);
        source.Play();

        if (!soundData.loop)
        {
            StartCoroutine(ReturnAudioSourceAfterPlay(source, soundData.clip.length));
        }

        return source;
    }

    public void StopSound(AudioSource source)
    {
        if (source != null && activeAudioSources.Contains(source))
        {
            ReturnAudioSource(source);
        }
    }

    public void StopAllSounds()
    {
        foreach (AudioSource source in activeAudioSources.ToArray())
        {
            ReturnAudioSource(source);
        }
    }

    public void StopSoundsByType(SoundType soundType)
    {
        foreach (AudioSource source in activeAudioSources.ToArray())
        {
            if (source.clip != null)
            {
                foreach (SoundData soundData in soundLibrary)
                {
                    if (soundData.clip == source.clip && soundData.soundType == soundType)
                    {
                        ReturnAudioSource(source);
                        break;
                    }
                }
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }

    private void ConfigureAudioSource(AudioSource source, SoundData soundData, Vector3 position)
    {
        source.clip = soundData.clip;
        source.volume = soundData.volume + Random.Range(-soundData.volumeVariation, soundData.volumeVariation);
        source.pitch = soundData.pitch + Random.Range(-soundData.pitchVariation, soundData.pitchVariation);
        source.loop = soundData.loop;
        source.playOnAwake = soundData.playOnAwake;
        source.spatialBlend = soundData.spatialBlend;
        source.minDistance = soundData.minDistance;
        source.maxDistance = soundData.maxDistance;

        if (position != default)
        {
            source.transform.position = position;
        }

        source.outputAudioMixerGroup = GetMixerGroup(soundData.soundType);
    }

    private AudioMixerGroup GetMixerGroup(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Music:
                return musicMixerGroup;
            case SoundType.SFX:
                return sfxMixerGroup;
            case SoundType.UI:
                return uiMixerGroup;
            case SoundType.Ambiance:
                return ambianceMixerGroup;
            case SoundType.Voice:
                return voiceMixerGroup;
            default:
                return null;
        }
    }

    private System.Collections.IEnumerator ReturnAudioSourceAfterPlay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (activeAudioSources.Contains(source))
        {
            ReturnAudioSource(source);
        }
    }
}
