using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static Action<string, bool> OnSocketStateChanged;
    public static Action<string, string> OnCubePlaced;
    public static Action OnPushButton;

    public bool _socketGreen;
    public bool _socketOrange;
    public bool _socketPurple;
    public bool _socketTool;

    public string _cubeInGreen;
    public string _cubeInOrange;
    public string _cubeInPurple;
    public string _cubeInTool;

    [SerializeField] private float _delayBeforeActivation = 5f;
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] private PiedestalUP _piedestal;

    private AsyncOperation _preloadedScene;
    private bool _isPreloading = false;

    private void OnEnable()
    {
        OnSocketStateChanged += OnSocketUpdate;
        OnCubePlaced += OnCubeUpdate;
        OnPushButton += LaunchStory;
    }

    private void OnDisable()
    {
        OnSocketStateChanged -= OnSocketUpdate;
        OnCubePlaced -= OnCubeUpdate;
        OnPushButton -= LaunchStory;
    }

    private void OnSocketUpdate(string socketName, bool state)
    {
        switch (socketName)
        {
            case "Green":
                _socketGreen = state;
                break;
            case "Orange":
                _socketOrange = state;
                break;
            case "Purple":
                _socketPurple = state;
                break;
            case "Tool":
                _socketTool = state;
                break;
        }
    }

    private void OnCubeUpdate(string socketName, string cubeName)
    {
        switch (socketName)
        {
            case "Green":
                _cubeInGreen = cubeName;
                break;
            case "Orange":
                _cubeInOrange = cubeName;
                break;
            case "Purple":
                _cubeInPurple = cubeName;
                break;
            case "Tool":
                _cubeInTool = cubeName;
                break;
        }
        StoryDirect();
    }

    public void LaunchStory()
    {
        Debug.Log($"{_levelManager.gameObject.name} launched something");
        switch (_levelManager._currentLevel)
        {
            case 0:
                CheckCombinationMenu();
                break;

            case 1:
                CheckCombinationBackstage();
                break;
        }
    }

    public void StoryDirect()
    {
        Debug.Log($"{_levelManager.gameObject.name} launched something");
        switch (_levelManager._currentLevel)
        {
            case 1:
                CheckDirectBackstage();
                break;
        }
    }

    private void CheckDirectBackstage()
    {
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {
            if (!_isPreloading)
            {
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                StartCoroutine(PreloadAndLaunchScene(2, _delayBeforeActivation));
            }
        }
    }

    private void CheckCombinationMenu()
    {
        Debug.Log("CombinaisonCheck");

        if (_cubeInGreen == "CubeGreen")
        {
            Debug.Log("Open Level 1");
            _levelManager.LoadLevel1();
        }
        else if (_cubeInGreen == "CubePurple")
        {
            Debug.Log("Quit");
            _levelManager.Quit();
        }
    }

    private void CheckCombinationBackstage()
    {
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(1);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(2);
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(3);
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(5);
        }
    }

    private IEnumerator PreloadAndLaunchScene(int buildIndex, float delayBeforeActivation)
    {
        _isPreloading = true;

        Debug.Log($"Début du préchargement de la scène {buildIndex}");

        _preloadedScene = SceneManager.LoadSceneAsync(buildIndex);
        _preloadedScene.allowSceneActivation = false;

        while (_preloadedScene.progress < 0.9f)
        {
            Debug.Log($"Chargement en cours: {_preloadedScene.progress * 100:F0}%");
            yield return null;
        }

        Debug.Log("Scène chargée à 90% - En attente...");

        yield return new WaitForSeconds(delayBeforeActivation);

        Debug.Log("Activation de la scène !");
        _preloadedScene.allowSceneActivation = true;

        _isPreloading = false;
    }
}
