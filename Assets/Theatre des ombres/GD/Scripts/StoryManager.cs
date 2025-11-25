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
    [SerializeField] private SceneTransitionManager _transition;

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
        switch (_levelManager._currentLevel)
        {
            case 1:
                CheckDirectBackstage();
                break;
        }
    }

    private void CheckDirectBackstage()
    {
        //Dialogue Chevalresse
        if (_cubeInGreen == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(1);
        }
        
        //Dialogue Squelette
        if (_cubeInGreen == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(2);
        }
        
        //Dialogue Roi
        if (_cubeInGreen == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(3);
        }
        
        //Dialogue Chevalresse Epée
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" )
        {
            _dialogueSequence.StartDialogueBranch(4);
        }
        
        //Dialogue Chevalresse Bouclier
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" )
        {
            _dialogueSequence.StartDialogueBranch(5);
        }
        
        //Dialogue Squelette Epée
        if (_cubeInGreen == "CubeGreen"  && _cubeInOrange == "Sword" )
        {
            _dialogueSequence.StartDialogueBranch(6);
        }
        
        //Dialogue Squelette Bouclier
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield")
        {
            _dialogueSequence.StartDialogueBranch(7);
        }
        
        //Dialogue Roi Epée
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" )
        {
            _dialogueSequence.StartDialogueBranch(8);
        }
        
        //Dialogue Roi Bouclier
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" )
        {
            _dialogueSequence.StartDialogueBranch(9);
        }

        //Dialogue Chevalresse Epée Squelette
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(10);
        }
        
        //Dialogue Chevalresse Epée Roi
        if (_cubeInGreen == "CubeOrange"  && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(11);
        }
        
        //Dialogue Chevalresse Bouclier Roi
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(12);
        }
        
        //Dialogue Chevalresse Bouclier Squelette
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(13);
        }
        
        //Dialogue Squelette Epee Roi
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(14);
        }
        
        //Dialogue Squelette Epee Chevalresse
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(15);
        }
        
        //Dialogue Squelette Bouclier Roi
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(16);
        }
        
        //Dialogue Squelette Bouclier Chevalresse
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(17);
        }
        
        //Dialogue Roi Epée Chevalresse
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(18);
        }
        
        //Dialogue Roi Epée Squelette
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(19);
        }
        
        //Dialogue Roi Bouclier Chevalresse
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(20);
        }
        
        //Dialogue Roi Bouclier Squelette
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
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
        //Dialogue Chevalresse Epée Squelette
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            
                _transition.StartCoroutine(_transition.TransitionToScene(2));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                
            
        }
        
        //Dialogue Chevalresse Epée Roi
        if (_cubeInGreen == "CubeOrange"  && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(3));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                
        }
        
        //Dialogue Chevalresse Bouclier Roi
        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(4));
                Debug.Log("Bonne combinaison ! Préchargement et lancement arès délai...");
            
        }
        
        //Dialogue Chevalresse Bouclier Squelette
       if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(5));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                
            
        }
        
        //Dialogue Squelette Epee Roi
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubePurple")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(6));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
           
        }
        
        //Dialogue Squelette Epee Chevalresse
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(7));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
               
        }
        
        //Dialogue Squelette Bouclier Roi
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubePurple")
        {
            
                _transition.StartCoroutine(_transition.TransitionToScene(8));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                
        }
        
        //Dialogue Squelette Bouclier Chevalresse
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(9));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
            
        }
        
        //Dialogue Roi Epée Chevalresse
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeOrange")
        {
                _transition.StartCoroutine(_transition.TransitionToScene(10));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                
        }
        
        //Dialogue Roi Epée Squelette
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Sword" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(11));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
                
           
        }
        
        //Dialogue Roi Bouclier Chevalresse
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeOrange")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(12));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
               
        }
        
        //Dialogue Roi Bouclier Squelette
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "Shield" && _cubeInPurple == "CubeGreen")
        {
            _transition.StartCoroutine(_transition.TransitionToScene(13));
                Debug.Log("Bonne combinaison ! Préchargement et lancement après délai...");
               
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
