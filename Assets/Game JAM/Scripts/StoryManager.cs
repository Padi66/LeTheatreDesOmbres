using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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
    
    
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] LevelManager _levelManager;
    


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
    
    //verification générale
    public void LaunchStory()
        {
            Debug.Log($" {_levelManager.gameObject.name} launched something");
            switch (_levelManager._currentLevel)
            {
                case 0:
                    CheckCombinationMenu();
                    break;

                case 1:
                    CheckCombinationLevel1();
                    break;

                case 2:
                    CheckCombinationLevel2();
                    break;
            }
        }
    
    //verification en direct
    public void StoryDirect()
    {
        Debug.Log($" {_levelManager.gameObject.name} launched something");
        switch (_levelManager._currentLevel)
        {
            case 1:
                CheckDirectLevel1();
                break;

            case 2:
                CheckDirectLevel2();
                break;
        }
    }


    private void CheckDirectLevel1()
    {
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {}
    }

    private void CheckDirectLevel2()
    {
        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {}
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

    private void CheckCombinationLevel1()
        {

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(1);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(2);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(3);
           
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(4);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(5);

            if (_dialogueSequence.dialogueFinished)
            {

                
            }
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(6);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }
    }

    private void CheckCombinationLevel2()
    {
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "CubeOrange" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(1);
            if (_dialogueSequence.dialogueFinished)
            {
                
                //YOU WIN
                _levelManager.LoadLevel3();
            }

        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubeGreen" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(2);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeGreen" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(3);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }

        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "CubePurple" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(4);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(5);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(6);
            if (_dialogueSequence.dialogueFinished)
            {
                
            }
        }
        
    }
}

    