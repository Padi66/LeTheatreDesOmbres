using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketOrange : MonoBehaviour
{
    //[SerializeField] private Light _lightcolor;
    [SerializeField] private XRSocketInteractor _socketInteractor;
    [SerializeField] private PiedestalUP _piedestal;
    

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
        string _cubeName = cube.name;

        XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    
        if (cube.GetComponent<CubeGreen>())
        {
            Debug.Log("Socket Orange contient Cube Vert");
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            Debug.Log("Socket Orange contient Cube Orange");
        }
        else if (cube.GetComponent<CubePurple>())
        {
            Debug.Log("Socket Orange contient Cube Violet");
        }
    
        StoryManager.OnSocketStateChanged?.Invoke("Orange", true);
        StoryManager.OnCubePlaced?.Invoke("Orange", _cubeName);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        //_lightcolor.intensity = 0;
        Debug.Log("Socket Orange vide");
        StoryManager.OnSocketStateChanged?.Invoke("Orange", false);
        StoryManager.OnCubePlaced?.Invoke("Orange", null);
    }

}