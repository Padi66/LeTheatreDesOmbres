using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StoryManager : MonoBehaviour
{
    public static Action<string, bool> OnSocketStateChanged;
    public static Action<string, string> OnCubePlaced;


    public bool _socketVert;
    public bool _socketOrange;
    public bool _socketViolet;

    public string _cubeInVert;
    public string _cubeInOrange;
    public string _cubeInViolet;

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
            case "Vert":
                _socketVert = state;
                break;
            case "Orange":
                _socketOrange = state;
                break;
            case "Purple":
                _socketViolet = state;
                break;
        }
    }

    private void OnCubeUpdate(string socketName, string cubeName)
    {
        
        switch (socketName)
        {
            case "Vert":
                _cubeInVert = cubeName;
                break;
            case "Orange":
                _cubeInOrange = cubeName;
                break;
            case "Purple":
                _cubeInViolet = cubeName;
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

        if (_cubeInVert == "CubeVert")
        {
            /*_curtainsLeft.OpenCurtains();
            _curtainsRight.OpenCurtains();*/
            _dialogueSequence.gameObject.SetActive(true);
            _dialogueSequence.StartDialogueBranch(1);

            Debug.Log("Fin du Garde du corps");

            
             _public.PublicReaction(true);
            
            
        }

        if (_cubeInVert == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(2);
            
                _public.PublicReaction(false);
            
        }
    }

    private void CheckCombinationLevel3()
        {

        if (_cubeInVert == "CubeViolet" && _cubeInOrange == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(1);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(true);
            }
        }
        if (_cubeInVert == "CubeVert" && _cubeInOrange == "CubeViolet")
        {
            _dialogueSequence.StartDialogueBranch(2);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeOrange" && _cubeInOrange == "CubeViolet")
        {
            _dialogueSequence.StartDialogueBranch(3);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeOrange" && _cubeInOrange == "CubeViolet")
        {
            _dialogueSequence.StartDialogueBranch(4);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeOrange" && _cubeInOrange == "CubeVert")
        {
            _dialogueSequence.StartDialogueBranch(5);

            if (_dialogueSequence.dialogueFinished)
            {

                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeViolet" && _cubeInOrange == "CubeOrange")
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
        if (_cubeInVert == "CubeVert" && _cubeInOrange == "CubeOrange" && _cubeInViolet == "CubeViolet")
        {
            _dialogueSequence.StartDialogueBranch(1);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(true);
                //YOU WIN
                _levelManager.LoadLevel3();
            }

        }

        if (_cubeInVert == "CubeOrange" && _cubeInOrange == "CubeVert" && _cubeInViolet == "CubeViolet")
        {
            _dialogueSequence.StartDialogueBranch(2);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeViolet" && _cubeInOrange == "CubeVert" && _cubeInViolet == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(3);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeVert" && _cubeInOrange == "CubeViolet" && _cubeInViolet == "CubeOrange")
        {
            _dialogueSequence.StartDialogueBranch(4);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeViolet" && _cubeInOrange == "CubeOrange" && _cubeInViolet == "CubeVert")
        {
            _dialogueSequence.StartDialogueBranch(5);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }

        if (_cubeInVert == "CubeOrange" && _cubeInOrange == "CubeViolet" && _cubeInViolet == "CubeVert")
        {
            _dialogueSequence.StartDialogueBranch(6);
            if (_dialogueSequence.dialogueFinished)
            {
                _public.PublicReaction(false);
            }
        }
        
    }
}

    