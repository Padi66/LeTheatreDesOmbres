using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketPurple : MonoBehaviour
{
    [SerializeField] private XRLockSocketInteractor _socketInteractor;
    [SerializeField] private StoryManager _storyManager;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private HapticManager _hapticManager;

    void Start()
    {
        _socketInteractor.enabled = false;
        
        if (_hapticManager == null)
        {
            _hapticManager = FindFirstObjectByType<HapticManager>();
        }
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
        string cubeName = null;

        if (_hapticManager != null)
        {
            _hapticManager.TriggerBothHands(0.6f, 0.2f);
        }

        if (cube.GetComponent<CubeGreen>())
        {
            cubeName = "CubeGreen";
            _particleSystem.Play();
            Debug.Log($"Socket Violet contient Cube Vert - nom envoyé: '{cubeName}'");
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            cubeName = "CubeOrange";
            _particleSystem.Play();
            Debug.Log($"Socket Violet contient Cube Orange - nom envoyé: '{cubeName}'");
        }
        else if (cube.GetComponent<CubePurple>())
        {
            cubeName = "CubePurple";
            _particleSystem.Play();
            Debug.Log($"Socket Violet contient Cube Violet - nom envoyé: '{cubeName}'");
        }

        StoryManager.OnSocketStateChanged?.Invoke("Purple", true);
        StoryManager.OnCubePlaced?.Invoke("Purple", cubeName);

        Debug.Log($"Event OnCubePlaced envoyé: Socket=Purple, Cube='{cubeName}'");
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        if (_hapticManager != null)
        {
            _hapticManager.TriggerBothHands(0.3f, 0.1f);
        }
        
        Debug.Log("Socket Violet vide");
        StoryManager.OnSocketStateChanged?.Invoke("Purple", false);
        StoryManager.OnCubePlaced?.Invoke("Purple", null);
    }

    public void LockCube()
    {
        if (_socketInteractor != null && _socketInteractor.hasSelection)
        {
            IXRSelectInteractable interactable = _socketInteractor.interactablesSelected[0];
            GameObject cube = interactable.transform.gameObject;

            XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
                Debug.Log($"Cube {cube.name} verrouillé - Grab désactivé");
            }

            _socketInteractor.enabled = false;
            Debug.Log("Socket désactivée - plus d'interactions possibles");
        }
    }
}
