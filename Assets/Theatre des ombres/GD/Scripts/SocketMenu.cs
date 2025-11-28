using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketMenu : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor _socketInteractor;
    

    public bool _isInSocket = false;

    void OnEnable()
    {
        _socketInteractor.selectEntered.AddListener(OnSelectEntered);
        _socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        _socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        _socketInteractor.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject cube = args.interactableObject.transform.gameObject;
        string cubeName = null;

        if (cube.GetComponent<CubeGreen>())
        {
            _isInSocket = true;
            cubeName = "CubeGreen";
            
            
        }
        else if (cube.GetComponent<CubePurple>())
        {
            _isInSocket = true;
            cubeName = "CubePurple";
            
            
        }

        StoryManager.OnSocketStateChanged?.Invoke("Purple", true);
        StoryManager.OnCubePlaced?.Invoke("Purple", cubeName);
        
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        _isInSocket = false;
        Debug.Log("Socket Menu vide");
        StoryManager.OnSocketStateChanged?.Invoke("Purple", false);
        StoryManager.OnCubePlaced?.Invoke("Purple", null);
    }
}

