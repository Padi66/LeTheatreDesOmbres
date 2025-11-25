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
        //if (args.interactableObject.transform.gameObject.GetComponent<CubeOrange>())

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

        if (cube.GetComponent<Sword>())
        {
            Debug.Log("Socket Tool contient Sword");
        }
        else if (cube.GetComponent<Shield>())
        {
            Debug.Log("Socket Tool contient Shield");
        }

        StoryManager.OnSocketStateChanged?.Invoke("Tool", true);
        StoryManager.OnCubePlaced?.Invoke("Tool", _cubeName);
    }
    

    void OnSelectExited(SelectExitEventArgs args)
    {
        //_lightcolor.intensity = 0;
        Debug.Log("Socket Orange vide");
        StoryManager.OnSocketStateChanged?.Invoke("Orange", false);
        StoryManager.OnCubePlaced?.Invoke("Orange", null);
    }

}