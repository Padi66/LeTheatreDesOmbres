using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


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
        CheckCombinationBackstage();
    }
    

    
    
    

    public void CheckDirectStep1()
    {
        //Chevalresse dans la forêt
        if (_cubeInGreen == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(2);
        }
        //Squelette dans la forêt
        else if (_cubeInGreen == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(3);
        }
        //Roi dans la forêt
        else if (_cubeInGreen == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(4);
        }
        
    }

    public void CheckDirectStep2()
    {
        //épée au village
        if (_cubeInOrange == "Sword")
        {
            _dialogueSequence.StartDialogueBranch(6);
        }
        //bouclier au village
        else if (_cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(7);
        }
    }

    public void CheckDirectStep3()
    {
        //Squelette au château
        if (_cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(9);
        }
        //Roi au château
        else if (_cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(10);
        }
        //Chevalresse au château
        else if (_cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(11);
        }

        
    }

    
    

    private void CheckCombinationBackstage()
    {
        
        //Chevalresse Epée Squelette
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(2));
            Debug.Log("Bonne combinaison ! //Chevalresse Epée Squelette");
        }
        
        //Chevalresse Epée Roi
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(3));
            Debug.Log("Bonne combinaison ! //Chevalresse Epée Roi");
        }
        
        //Chevalresse Bouclier Roi
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(4));
            Debug.Log("Bonne combinaison ! //Chevalresse Bouclier Roi");
        }

        //Chevalresse Bouclier Squelette
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(5));
            Debug.Log("Bonne combinaison ! //Chevalresse Bouclier Squelette");
        }
        
        //Squelette Epée Roi
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(6));
            Debug.Log("Bonne combinaison ! //Squelette Epée Roi");
        }
        
        //Squelette Epee Chevalier
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(7));
            Debug.Log("Bonne combinaison ! //Squelette Epee Chevalier");
        }

        //Squelette Bouclier Roi
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(8));
            Debug.Log("Bonne combinaison !  //Squelette Bouclier Roi");
        }
        
        //Squelette Bouclier Chevalresse
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(9));
            Debug.Log("Bonne combinaison ! //Squelette Bouclier Chevalresse");
        }

        //Roi Epée Chevalresse
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(10));
            Debug.Log("Bonne combinaison !  //Roi Epée Chevalresse");
        }
        
        //Roi Epee Squelette
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(11));
            Debug.Log("Bonne combinaison !//Roi Epee Squelette");
        }

        //Roi Bouclier Chevalresse
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(12));
            Debug.Log("Bonne combinaison ! //Roi Bouclier Chevalresse");
        }
        
        //Roi Bouclier Squelette
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(13));
            Debug.Log("Bonne combinaison ! //Roi Bouclier Squelette");
        }

        else
        {
            Debug.Log("Aucune Combinaison");
        }
    }
}
