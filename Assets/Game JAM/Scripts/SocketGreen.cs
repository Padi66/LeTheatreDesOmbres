using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketGreen : MonoBehaviour
{
    //[SerializeField] private Light _lightcolor;
    [SerializeField] private XRSocketInteractor _socketInteractor;
    

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
        
        //SON
       
        GameObject cube = args.interactableObject.transform.gameObject;
        string _cubeName = cube.name;
        //_lightcolor.intensity = 1;
        
        if (cube.GetComponent<CubeGreen>())
        {
            Debug.Log("Socket Vert contient Cube Vert");
            //Faire monter le next piedestal
            
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            Debug.Log("Socket Vert contient Cube Orange");
            //Faire monter le next piedestal
            
        }
        else if (cube.GetComponent<CubePurple>())
        {
            Debug.Log("Socket Vert contient Cube Violet");
            //Faire monter le next piedestal
            
        }
        
        StoryManager.OnSocketStateChanged?.Invoke("Vert", true);
        StoryManager.OnCubePlaced?.Invoke("Vert", _cubeName);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        //SON
        //_lightcolor.intensity = 0;
        Debug.Log("Socket Vert vide");
        StoryManager.OnSocketStateChanged?.Invoke("Vert", false);
        StoryManager.OnCubePlaced?.Invoke("Vert", null);
    }

}