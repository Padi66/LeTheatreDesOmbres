using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketMenu : MonoBehaviour
{
    //[SerializeField] private Light _lightcolor;
    [SerializeField] private XRSocketInteractor _socketInteractor;
    public bool _isInSocket = false ;
    

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
        //if (args.interactableObject.transform.gameObject.GetComponent<CubeOrange>())
        
        GameObject cube = args.interactableObject.transform.gameObject;
        string _cubeName = cube.name;
        
        
        if (cube.GetComponent<CubeGreen>())
        {
            Debug.Log("Socket Violet contient Cube Vert");
            _isInSocket = true;
            
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            Debug.Log("Socket Violet contient Cube Orange");
            _isInSocket = true;
        }
        else if (cube.GetComponent<CubePurple>())
        {
            Debug.Log("Socket Violet contient Cube Violet");
            _isInSocket = true;
        }
        
        StoryManager.OnSocketStateChanged?.Invoke("Purple", true);
        StoryManager.OnCubePlaced?.Invoke("Purple", _cubeName);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        _isInSocket = false;
        //_lightcolor.intensity = 0;
        Debug.Log("Socket Violet vide");
        StoryManager.OnSocketStateChanged?.Invoke("Purple", false);
        StoryManager.OnCubePlaced?.Invoke("Purple", null);
    }
}

