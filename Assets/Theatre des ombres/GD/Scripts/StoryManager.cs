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
        CheckDirectStep1();
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
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword")
        {
            _dialogueSequence.StartDialogueBranch(4);
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(5);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword")
        {
            _dialogueSequence.StartDialogueBranch(6);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(7);
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword")
        {
            _dialogueSequence.StartDialogueBranch(8);
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(9);
        }
    }

    public void CheckDirectStep3()
    {
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(10);
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(11);
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(12);
        }
        else if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(13);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(14);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(15);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(16);
        }
        else if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(17);
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(18);
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(19);
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(20);
        }
        else if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(21);
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
