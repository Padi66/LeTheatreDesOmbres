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
    [SerializeField] private HapticManager _hapticManager;
    private bool _hasDone = false;
    
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
            bool isLeftHand = args.interactorObject.transform.name.Contains("Left");
            
            if (isLeftHand)
                _hapticManager.TriggerLeftHand(0.6f, 0.2f);
            else
                _hapticManager.TriggerRightHand(0.6f, 0.2f);
        }
    
        if (cube.GetComponent<CubeGreen>())
        {
            cubeName = "CubeGreen";
            _particleSystem.Play();
            Debug.Log($"Socket Vert contient Cube Vert - nom envoyé: '{cubeName}'");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    Debug.Log($"Appel UpOrange avec socket: {_socketInteractor.enabled}");
                    _piedestal.UpOrange();
                    _hasDone = true;
                }
            }
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            cubeName = "CubeOrange";
            _particleSystem.Play();
            Debug.Log($"Socket Vert contient Cube Orange - nom envoyé: '{cubeName}'");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpOrange();
                    _hasDone = true;
                }
            }
        }
        else if (cube.GetComponent<CubePurple>())
        {
            cubeName = "CubePurple";
            _particleSystem.Play();
            Debug.Log($"Socket Vert contient Cube Violet - nom envoyé: '{cubeName}'");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpOrange();
                    _hasDone = true;
                }
            }
        }
    
        StoryManager.OnSocketStateChanged?.Invoke("Green", true);
        StoryManager.OnCubePlaced?.Invoke("Green", cubeName);
    
        Debug.Log($"Event OnCubePlaced envoyé: Socket=Green, Cube='{cubeName}'");
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("Socket Vert vide");
        StoryManager.OnSocketStateChanged?.Invoke("Green", false);
        StoryManager.OnCubePlaced?.Invoke("Green", null);
    }
}
