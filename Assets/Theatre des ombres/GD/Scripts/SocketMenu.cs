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

    

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject cube = args.interactableObject.transform.gameObject;
        string cubeName = null;

        if (cube.GetComponent<PlayTicket>())
        {
            _isInSocket = true;
           
            
            
        }
        else if (cube.GetComponent<QuitTicket>())
        {
            _isInSocket = true;
            
            
            
        }

    
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        _isInSocket = false;
        Debug.Log("Socket Menu vide");
       
    }
}

