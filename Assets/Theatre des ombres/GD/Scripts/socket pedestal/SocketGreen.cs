using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class SocketGreen : MonoBehaviour
{
    [SerializeField] private XRLockSocketInteractor _socketInteractor;
    [SerializeField] private PiedestalUP _piedestal;
    [SerializeField] private StoryManager _storyManager;
    [SerializeField] private ParticleSystem _particleSystem;
    private bool _hasDone = false;
    
    void Start()
    {
       _socketInteractor.enabled = false; 
       _piedestal.UpGreen();
    }

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
            _particleSystem.Play();
            Debug.Log("Socket Vert contient Cube Vert");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    Debug.Log($"Appel UpOrange avec socket: {_socketInteractor.enabled}");
                    _piedestal.UpOrange();
                    _hasDone = true;
                }
            }
            _storyManager.CheckDirectStep1();
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            _particleSystem.Play();
            Debug.Log("Socket Vert contient Cube Orange");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpOrange();
                    _hasDone = true;
                }
            }
            _storyManager.CheckDirectStep1();
        }
        else if (cube.GetComponent<CubePurple>())
        {
            _particleSystem.Play();
            Debug.Log("Socket Vert contient Cube Violet");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpOrange();
                    _hasDone = true;
                }
            }
            _storyManager.CheckDirectStep1();
        }
        
        StoryManager.OnSocketStateChanged?.Invoke("Green", true);
        StoryManager.OnCubePlaced?.Invoke("Green", _cubeName);
        
        /*if (_lockCoroutine != null)
        {
            StopCoroutine(_lockCoroutine);
        }
        _lockCoroutine = StartCoroutine(LockCubeAfterDelay(cube));*/
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        /*GameObject cube = args.interactableObject.transform.gameObject;
        
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
            _piedestal.DownGreen();
        }*/
        
        Debug.Log("Socket Vert vide");
        StoryManager.OnSocketStateChanged?.Invoke("Green", false);
        StoryManager.OnCubePlaced?.Invoke("Green", null);
    }

    /*private IEnumerator LockCubeAfterDelay(GameObject cube)
    {
        yield return new WaitForSeconds(_lockDelay);
        
        XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            Debug.Log("Cube verrouillé dans Socket Vert");
        }

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        if (_socketInteractor != null)
        {
            _socketInteractor.enabled = false;
            Debug.Log("Socket Vert désactivé");
        }
        
        _lockCoroutine = null;
    }*/
}
