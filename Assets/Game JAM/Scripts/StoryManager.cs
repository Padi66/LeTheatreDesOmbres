using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StoryManager : MonoBehaviour
{
    public static Action<string, bool> OnSocketStateChanged;
    public static Action<string, string> OnCubePlaced;


    public bool _socketGreen;
    public bool _socketOrange;
    public bool _socketPurple;

    public string _cubeInGreen;
    public string _cubeInOrange;
    public string _cubeInPurple;

    public bool _winOrLoose;
    
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] Public _public;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] TeleportationEvent _teleportationEvent;


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
    }

    public void LaunchStory()
        {
          
                //faire un truc
            Debug.Log($" {_levelManager.gameObject.name} launched something");
            switch (_levelManager._currentLevel)
            {
                case 1:
                    CheckCombinationLevel1();
                    break;

                case 2:
                    CheckCombinationLevel2();
                    break;

                case 3:
                    CheckCombinationLevel3();
                    break;
            }
        }
    

    private void CheckCombinationLevel1()
    {
        Debug.Log("CombinaisonCheck");

        if (_cubeInGreen == "CubeGreen")
        {
            /*_curtainsLeft.OpenCurtains();
            _curtainsRight.OpenCurtains();*/
            _dialogueSequence.gameObject.SetActive(true);
            _dialogueSequence.StartDialogueBranch(1);

            Debug.Log("Fin du Garde du corps");

            
             _public.PublicReaction(true);
            
            
        }

        if (_cubeInGreen == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(2);
            
                _public.PublicReaction(false);
            
        }
    }

    private void CheckCombinationLevel3()
        {

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(1);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(true);
            }
        }
        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(2);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(3);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(4);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(5);

            if (_dialogueSequence.dialogueFinished)
            {

                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(6);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
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
                _public.PublicReaction(true);
                //YOU WIN
                _levelManager.LoadLevel3();
            }

        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubeGreen" && _cubeInPurple == "CubePurple")
        {
            _dialogueSequence.StartDialogueBranch(2);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeGreen" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(3);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubeGreen" && _cubeInOrange == "CubePurple" && _cubeInPurple == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(4);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubePurple" && _cubeInOrange == "CubeOrange" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(5);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInGreen == "CubeOrange" && _cubeInOrange == "CubePurple" && _cubeInPurple == "CubeGreen")
        {
            _dialogueSequence.StartDialogueBranch(6);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }
        
    }
}

    