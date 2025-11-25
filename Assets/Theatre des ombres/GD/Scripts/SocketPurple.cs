using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketPurple : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor _socketInteractor;
    [SerializeField] private PiedestalUP _piedestal;
    [SerializeField] private float _lockDelay = 0.3f;
    
    private Coroutine _lockCoroutine;

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
        
        if (cube.GetComponent<CubeGreen>())
        {
            Debug.Log("Socket Violet contient Cube Vert");
            if (_piedestal != null)
            {
                _piedestal.UpPurple();
            }
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            Debug.Log("Socket Violet contient Cube Orange");
            if (_piedestal != null)
            {
                _piedestal.UpPurple();
            }
        }
        else if (cube.GetComponent<CubePurple>())
        {
            Debug.Log("Socket Violet contient Cube Violet");
            if (_piedestal != null)
            {
                _piedestal.UpPurple();
            }
        }
        
        StoryManager.OnSocketStateChanged?.Invoke("Purple", true);
        StoryManager.OnCubePlaced?.Invoke("Purple", _cubeName);
        
        if (_lockCoroutine != null)
        {
            StopCoroutine(_lockCoroutine);
        }
        _lockCoroutine = StartCoroutine(LockCubeAfterDelay(cube));
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        GameObject cube = args.interactableObject.transform.gameObject;
        
        if (_lockCoroutine != null)
        {
            StopCoroutine(_lockCoroutine);
            _lockCoroutine = null;
        }
        
        XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        
        if (_piedestal != null)
        {
            _piedestal.DownPurple();
        }
        
        Debug.Log("Socket Violet vide");
        StoryManager.OnSocketStateChanged?.Invoke("Purple", false);
        StoryManager.OnCubePlaced?.Invoke("Purple", null);
    }

    private IEnumerator LockCubeAfterDelay(GameObject cube)
    {
        yield return new WaitForSeconds(_lockDelay);
        
        XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            Debug.Log("Cube verrouillé dans Socket Violet");
        }

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        _lockCoroutine = null;
    }
}
