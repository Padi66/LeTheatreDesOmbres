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
    [SerializeField] AudioSource _audioSource;
    /*[SerializeField] private AudioClip Fixe1;
    [SerializeField] private AudioClip Fixe2;
    [SerializeField] private AudioClip Fixe3;
    [SerializeField] private AudioClip Squelette1;
    [SerializeField] private AudioClip Squelette2;
    [SerializeField] private AudioClip Roi1;
    [SerializeField] private AudioClip Roi2;
    [SerializeField] private AudioClip chevaleresse1;
    [SerializeField] private AudioClip chevaleresse2;
    [SerializeField] private AudioClip bouclier;
    [SerializeField] private AudioClip epee;*/
    [SerializeField] private float audioCooldown = 1f;

    private float _lastGreenAudioTime = -999f;
    private float _lastOrangeAudioTime = -999f;
    private float _lastPurpleAudioTime = -999f;

    

    [Header("Transition Settings")]
    
    [SerializeField] private float delayBeforeTransition = 1f;
    private AsyncOperation _preloadedScene;
    private int pendingSceneIndex = -1;
    private bool waitingForDialogues = false;
    public bool _isLaunched = false;

    private void Start()
    {

        /*if (_dialogueSequence != null)
        {
            _dialogueSequence.onAllDialoguesComplete.AddListener(OnAllDialoguesComplete);
        }
        else
        {
            Debug.LogWarning("DialogueSequence not assigned in StoryManager!");
        }*/
        _dialogueSequence.StartDialogueBranch(1);
        //_audioSource.PlayOneShot(Fixe1);
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
        if (Time.time - _lastGreenAudioTime < audioCooldown)
            return;

        if (_cubeInGreen == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(2);
            //_audioSource.PlayOneShot(chevaleresse1);
            _lastGreenAudioTime = Time.time;
        }
        else if (_cubeInGreen == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(3);
            //_audioSource.PlayOneShot(Squelette1);
            _lastGreenAudioTime = Time.time;
        }
        else if (_cubeInGreen == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(4);
            //_audioSource.PlayOneShot(Roi1);
            _lastGreenAudioTime = Time.time;
        }
    }


    public void CheckDirectStep2()
    {
        if (Time.time - _lastOrangeAudioTime < audioCooldown)
            return;

        if (_cubeInOrange == "Sword")
        {
            _dialogueSequence.StartDialogueBranch(6);
            //_audioSource.PlayOneShot(epee);
            _lastOrangeAudioTime = Time.time;
        }
        else if (_cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(7);
            //_audioSource.PlayOneShot(bouclier);
            _lastOrangeAudioTime = Time.time;
        }
    }


    public void CheckDirectStep3()
    {
        if (Time.time - _lastPurpleAudioTime < audioCooldown)
            return;

        if (_cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(9);
            //_audioSource.PlayOneShot(Squelette2);
            _lastPurpleAudioTime = Time.time;
        }
        else if (_cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(10);
            //_audioSource.PlayOneShot(Roi2);
            _lastPurpleAudioTime = Time.time;
        }
        else if (_cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(11);
            //_audioSource.PlayOneShot(chevaleresse2);
            _lastPurpleAudioTime = Time.time;
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
            Debug.Log($"✅ Lancement de la transition vers scène {targetScene} dans {delayBeforeTransition}s");
            StartCoroutine(TransitionAfterDelay(targetScene));
            /*pendingSceneIndex = targetScene;
            waitingForDialogues = true;
            Debug.Log($"Combinaison détectée! Attente de la fin des dialogues avant de charger la scène {targetScene}");*/
        }
    }

    /*private void OnAllDialoguesComplete()
    {
        if (!waitingForDialogues || pendingSceneIndex < 0)
        {
            Debug.Log("Tous les dialogues terminés, mais aucune transition en attente.");
            return;
        }

        Debug.Log($"Tous les dialogues terminés! Lancement de la transition vers la scène {pendingSceneIndex} dans {delayBeforeTransition}s");
        StartCoroutine(TransitionAfterDelay());
    }*/

    private IEnumerator TransitionAfterDelay(int sceneIndex)
    {
        Debug.Log($"[StoryManager] Waiting {delayBeforeTransition}s before transition...");
        yield return new WaitForSeconds(delayBeforeTransition);

        Debug.Log($"[StoryManager] Starting transition to scene {sceneIndex}");
    
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.StartCoroutine(
                SceneTransitionManager.Instance.TransitionToScene(sceneIndex, disableMovement: true)
            );
        }
        else
        {
            Debug.LogError("[StoryManager] SceneTransitionManager.Instance is null!");
        }
    }



}
