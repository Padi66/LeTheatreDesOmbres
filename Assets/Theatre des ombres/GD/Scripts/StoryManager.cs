using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class StoryManager : MonoBehaviour
{
    public static Action<string, bool> OnSocketStateChanged;
    public static Action<string, string> OnCubePlaced;
    public static Action OnPushButton;

    public bool _socketGreen;
    public bool _socketOrange;
    public bool _socketPurple;

    public string _cubeInGreen;
    public string _cubeInOrange;
    public string _cubeInPurple;

    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] private PiedestalUP _piedestal;
    [SerializeField] private SceneTransitionManager _transition;

    private AsyncOperation _preloadedScene;

    private void Start()
    {
        StartCoroutine(TestHapticFeedback());
    }

    private IEnumerator TestHapticFeedback()
    {
        yield return new WaitForSeconds(1f);

        List<InputDevice> rightDevices = new List<InputDevice>();
        List<InputDevice> leftDevices = new List<InputDevice>();

        InputDeviceCharacteristics rightHandedCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDeviceCharacteristics leftHandedCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightHandedCharacteristics, rightDevices);
        InputDevices.GetDevicesWithCharacteristics(leftHandedCharacteristics, leftDevices);

        List<InputDevice> allDevices = new List<InputDevice>();
        allDevices.AddRange(rightDevices);
        allDevices.AddRange(leftDevices);

        Debug.Log($"Found {allDevices.Count} controllers (Right: {rightDevices.Count}, Left: {leftDevices.Count})");

        foreach (InputDevice device in allDevices)
        {
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float amplitude = 0.5f;
                    float duration = 0.3f;
                    bool success = device.SendHapticImpulse(channel, amplitude, duration);
                    Debug.Log($"Haptic feedback sent to {device.name}: amplitude={amplitude}, duration={duration}, success={success}");
                }
                else
                {
                    Debug.LogWarning($"Device {device.name} does not support haptic impulse");
                }
            }
        }
    }

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
        Debug.Log($"OnSocketUpdate: Socket={socketName}, State={state}");

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
        }
    }

    private void OnCubeUpdate(string socketName, string cubeName)
    {
        Debug.Log($"OnCubeUpdate: Socket={socketName}, Cube='{cubeName}'");

        switch (socketName)
        {
            case "Green":
                _cubeInGreen = cubeName;
                Debug.Log($"_cubeInGreen = '{_cubeInGreen}'");
                break;
            case "Orange":
                _cubeInOrange = cubeName;
                Debug.Log($"_cubeInOrange = '{_cubeInOrange}'");
                break;
            case "Purple":
                _cubeInPurple = cubeName;
                Debug.Log($"_cubeInPurple = '{_cubeInPurple}'");
                break;
        }
    }

    public void ExecuteMenuActionWithCubeType(string cubeType)
    {
        Debug.Log($"=== ExecuteMenuActionWithCubeType appelé - Type: '{cubeType}' ===");

        if (string.IsNullOrEmpty(cubeType))
        {
            Debug.LogWarning("Type de cube vide!");
            return;
        }

        if (cubeType == "CubeGreen")
        {
            Debug.Log("✓ Cube VERT - Chargement Level 1");
            _levelManager.LoadLevel1();
        }
        else if (cubeType == "CubePurple")
        {
            Debug.Log("✓ Cube VIOLET - Fermeture du jeu");
            _levelManager.Quit();
        }
        else
        {
            Debug.LogWarning($"Type de cube non géré: '{cubeType}'");
        }
    }

    public void ExecuteMenuAction()
    {
        Debug.Log("=== ExecuteMenuAction appelé ===");
        Debug.Log($"_cubeInPurple = '{_cubeInPurple}'");

        if (string.IsNullOrEmpty(_cubeInPurple))
        {
            Debug.LogWarning("Aucun cube dans la socket Purple!");
            return;
        }

        if (_cubeInPurple == "CubeGreen")
        {
            Debug.Log("✓ Cube VERT - Chargement Level 1 MAINTENANT");
            _levelManager.LoadLevel1();
        }
        else if (_cubeInPurple == "CubePurple")
        {
            Debug.Log("✓ Cube VIOLET - Fermeture du jeu MAINTENANT");
            _levelManager.Quit();
        }
        else
        {
            Debug.LogWarning($"Cube non reconnu: '{_cubeInPurple}'");
        }
    }

    public void LaunchStory()
    {
        Debug.Log($"=== LaunchStory appelé ===");
        Debug.Log($"Current Level: {_levelManager._currentLevel}");
        Debug.Log($"_cubeInGreen: '{_cubeInGreen}'");
        Debug.Log($"_cubeInOrange: '{_cubeInOrange}'");
        Debug.Log($"_cubeInPurple: '{_cubeInPurple}'");

        switch (_levelManager._currentLevel)
        {
            case 0:
                Debug.Log("→ Appel de CheckCombinationMenu()");
                CheckCombinationMenu();
                break;

            case 1:
                Debug.Log("→ Appel de CheckCombinationBackstage()");
                CheckCombinationBackstage();
                break;

            default:
                Debug.LogWarning($"Level {_levelManager._currentLevel} non géré!");
                break;
        }
    }

    public void StoryDirect()
    {
        Debug.Log($"{_levelManager.gameObject.name} launched something");
    }

    public void CheckDirectStep1()
    {
        if (_cubeInGreen == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(1);
        }
        else if (_cubeInGreen == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(2);
        }
        else if (_cubeInGreen == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(3);
        }
    }

    public void CheckDirectStep2()
    {
        if (_cubeInOrange == "Sword")
        {

        }
        else if (_cubeInOrange == "Shield")
        {

        }
    }

    public void CheckDirectStep3()
    {
        if (_cubeInPurple == "CubeGreen")
        {

        }
        else if (_cubeInPurple == "CubePurple")
        {

        }
        else if (_cubeInPurple == "CubeOrange")
        {

        }
    }

    private void CheckCombinationMenu()
    {
        Debug.Log($"=== CheckCombinationMenu appelé ===");
        Debug.Log($"_cubeInPurple = '{_cubeInPurple}'");
        Debug.Log($"_currentLevel = {_levelManager._currentLevel}");

        if (string.IsNullOrEmpty(_cubeInPurple))
        {
            Debug.LogWarning("Aucun cube dans la socket Purple!");
            return;
        }

        if (_cubeInPurple == "CubeGreen")
        {
            Debug.Log("✓ Cube VERT détecté - Chargement Level 1");
            _levelManager.LoadLevel1();
        }
        else if (_cubeInPurple == "CubePurple")
        {
            Debug.Log("✓ Cube VIOLET détecté - Fermeture du jeu");
            _levelManager.Quit();
        }
        else
        {
            Debug.LogWarning($"Cube non reconnu: '{_cubeInPurple}'");
        }
    }

    private void CheckCombinationBackstage()
    {
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(2));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(3));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(4));
            Debug.Log("Bonne combinaison ! Préchargement et lancement arès délai...");
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(5));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(6));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(7));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(8));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(9));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(10));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(11));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(12));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(13));
            Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
        }
    }
}
