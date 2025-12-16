using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusicManager : MonoBehaviour
{
    public static PersistentMusicManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuBackstageMusic;
    

    [Header("Settings")]
    [SerializeField] private float musicVolume = 0.3f;
    [SerializeField] private bool playOnAwake = true;

    [Header("Scene Configuration")]
    [SerializeField] private string[] scenesWithMusic = { "TEST MAIN MENU", "TEST BACKSTAGE" };

    [Header("Positioning")]
    [SerializeField] private bool autoReposition = true;
    [SerializeField] private string menuSceneName = "TEST MAIN MENU";
    [SerializeField] private string backstageSceneName = "TEST BACKSTAGE";
    [SerializeField] private Vector3 menuPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 backstagePosition = new Vector3(0f, 2f, 0f);

    private Transform originalParent;
    private Vector3 originalLocalPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SaveOriginalTransform();
            InitializeAudioSource();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (playOnAwake)
        {
            PlayMenuMusic();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SaveOriginalTransform()
    {
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
    }

    private void InitializeAudioSource()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        musicSource.spatialBlend = 1f;
        musicSource.minDistance = 5f;
        musicSource.maxDistance = 50f;
        musicSource.rolloffMode = AudioRolloffMode.Linear;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (autoReposition)
        {
            HandleRepositioning(scene.name);
        }

        bool shouldPlayMusic = IsSceneInList(scene.name, scenesWithMusic);

        if (shouldPlayMusic)
        {
            if (!musicSource.isPlaying)
            {
                PlayMenuMusic();
            }
        }
        else
        {
            StopMusic();
        }
    }

    private void HandleRepositioning(string sceneName)
    {
        if (sceneName == backstageSceneName)
        {
            RepositionToBackstage();
        }
        else if (sceneName == menuSceneName)
        {
            RepositionToMenu();
        }
    }

    private void RepositionToBackstage()
    {
        GameObject musicPosition = GameObject.Find("MusicSourcePosition");
        
        if (musicPosition != null)
        {
            transform.SetParent(null);
            transform.position = musicPosition.transform.position;
            transform.rotation = musicPosition.transform.rotation;
            Debug.Log("MusicManager repositionné au marqueur MusicSourcePosition dans Backstage");
            return;
        }

        GameObject backstageParent = GameObject.Find("Backstage");
        
        if (backstageParent != null)
        {
            transform.SetParent(backstageParent.transform);
            transform.localPosition = backstagePosition;
            Debug.Log($"MusicManager repositionné dans Backstage à {backstagePosition}");
        }
        else
        {
            transform.SetParent(null);
            transform.position = backstagePosition;
            Debug.LogWarning("GameObject 'Backstage' non trouvé, positionnement en coordonnées monde");
        }
    }

    private void RepositionToMenu()
    {
        GameObject musicPosition = GameObject.Find("MusicSourcePosition");
        
        if (musicPosition != null)
        {
            transform.SetParent(null);
            transform.position = musicPosition.transform.position;
            transform.rotation = musicPosition.transform.rotation;
            Debug.Log("MusicManager repositionné au marqueur MusicSourcePosition dans Menu");
            return;
        }

        transform.SetParent(originalParent);
        
        if (originalParent != null)
        {
            transform.localPosition = originalLocalPosition;
            Debug.Log($"MusicManager repositionné à sa position d'origine dans le Menu");
        }
        else
        {
            transform.position = menuPosition;
            Debug.Log($"MusicManager repositionné à {menuPosition} dans le Menu");
        }
    }

    private bool IsSceneInList(string sceneName, string[] sceneList)
    {
        foreach (string scene in sceneList)
        {
            if (sceneName.Contains(scene) || scene == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetPositionByTransform(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }

    public void PlayMenuMusic()
    {
        if (menuBackstageMusic != null && musicSource != null)
        {
            if (musicSource.clip != menuBackstageMusic)
            {
                musicSource.clip = menuBackstageMusic;
                musicSource.Play();
            }
            else if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }
    
    

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void FadeOut(float duration = 1f)
    {
        if (musicSource != null)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
    }

    public void FadeIn(float duration = 1f)
    {
        if (musicSource != null)
        {
            StartCoroutine(FadeInCoroutine(duration));
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
        musicSource.volume = musicVolume;
    }

    private System.Collections.IEnumerator FadeInCoroutine(float duration)
    {
        musicSource.volume = 0f;
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, elapsed / duration);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }
}
