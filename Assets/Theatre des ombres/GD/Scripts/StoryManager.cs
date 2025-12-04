using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

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
    

    [Header("Transition Settings")]
    [SerializeField] private float delayBeforeTransition = 1f;

    private AsyncOperation _preloadedScene;
    private int pendingSceneIndex = -1;
    private bool waitingForDialogues = false;
    public bool _isLaunched = false;

    private void Start()
    {
        StartCoroutine(TestHapticFeedback());

        if (_dialogueSequence != null)
        {
            _dialogueSequence.onAllDialoguesComplete.AddListener(OnAllDialoguesComplete);
        }
        else
        {
            Debug.LogWarning("DialogueSequence not assigned in StoryManager!");
        }
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
    }

    private void OnDisable()
    {
        OnSocketStateChanged -= OnSocketUpdate;
        OnCubePlaced -= OnCubeUpdate;
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
                CheckDirectStep1();
                break;
            case "Orange":
                _cubeInOrange = cubeName;
                CheckDirectStep2();
                break;
            case "Purple":
                _cubeInPurple = cubeName;
                CheckDirectStep3();
                break;
        }
        
    }

    public void CheckDirectStep1()
    {
        if (_cubeInGreen == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(2);
        }
        else if (_cubeInGreen == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(3);
        }
        else if (_cubeInGreen == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(4);
        }
    }

    public void CheckDirectStep2()
    {
        if (_cubeInOrange == "Sword")
        {
            _dialogueSequence.StartDialogueBranch(6);
        }
        else if (_cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(7);
        }
    }

    public void CheckDirectStep3()
    {
        if (_cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(9);
        }
        else if (_cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(10);
        }
        else if (_cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(11);
        }
    }

    public void CheckCombinationBackstage()
    {
        
        int targetScene = -1;

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _isLaunched = true;
            targetScene = 2;
            Debug.Log("Bonne combinaison ! //Chevalresse Epée Squelette");
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _isLaunched = true;
            targetScene = 3;
            Debug.Log("Bonne combinaison ! //Chevalresse Epée Roi");
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _isLaunched = true;
            targetScene = 4;
            Debug.Log("Bonne combinaison ! //Chevalresse Bouclier Roi");
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _isLaunched = true;
            targetScene = 5;
            Debug.Log("Bonne combinaison ! //Chevalresse Bouclier Squelette");
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _isLaunched = true;
            targetScene = 6;
            Debug.Log("Bonne combinaison ! //Squelette Epée Roi");
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _isLaunched = true;
            targetScene = 7;
            Debug.Log("Bonne combinaison ! //Squelette Epee Chevalier");
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _isLaunched = true;
            targetScene = 8;
            Debug.Log("Bonne combinaison !  //Squelette Bouclier Roi");
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _isLaunched = true;
            targetScene = 9;
            Debug.Log("Bonne combinaison ! //Squelette Bouclier Chevalresse");
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _isLaunched = true;
            targetScene = 10;
            Debug.Log("Bonne combinaison !  //Roi Epée Chevalresse");
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _isLaunched = true;
            targetScene = 11;
            Debug.Log("Bonne combinaison !//Roi Epee Squelette");
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _isLaunched = true;
            targetScene = 12;
            Debug.Log("Bonne combinaison ! //Roi Bouclier Chevalresse");
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _isLaunched = true;
            targetScene = 13;
            Debug.Log("Bonne combinaison ! //Roi Bouclier Squelette");
        }
        else
        {
            Debug.Log("Aucune Combinaison");
        }

        if (targetScene >= 0)
        {
            pendingSceneIndex = targetScene;
            waitingForDialogues = true;
            Debug.Log($"Combinaison détectée! Attente de la fin des dialogues avant de charger la scène {targetScene}");
        }
    }

    private void OnAllDialoguesComplete()
    {
        if (!waitingForDialogues || pendingSceneIndex < 0)
        {
            Debug.Log("Tous les dialogues terminés, mais aucune transition en attente.");
            return;
        }

        Debug.Log($"Tous les dialogues terminés! Lancement de la transition vers la scène {pendingSceneIndex} dans {delayBeforeTransition}s");
        StartCoroutine(TransitionAfterDelay());
    }

    private IEnumerator TransitionAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeTransition);

        if (_transition != null)
        {
            _transition.StartCoroutine(_transition.TransitionToScene(pendingSceneIndex));
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager not found! Loading scene directly.");
            SceneManager.LoadScene(pendingSceneIndex);
        }

        pendingSceneIndex = -1;
        waitingForDialogues = false;
    }
}
