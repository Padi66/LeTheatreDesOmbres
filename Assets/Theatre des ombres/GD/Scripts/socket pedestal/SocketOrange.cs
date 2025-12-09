using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketOrange : MonoBehaviour
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
            _hapticManager.TriggerBothHands(0.6f, 0.2f);
        }

        if (cube.GetComponent<Sword>())
        {
            cubeName = "Sword";
            _particleSystem.Play();
            Debug.Log($"Socket Orange contient Sword - nom envoyé: '{cubeName}'");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpPurple();
                    _hasDone = true;
                }
            }
        }
        else if (cube.GetComponent<Shield>())
        {
            cubeName = "Shield";
            _particleSystem.Play();
            Debug.Log($"Socket Orange contient Shield - nom envoyé: '{cubeName}'");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpPurple();
                    _hasDone = true;
                }
            }
        }
    
        StoryManager.OnSocketStateChanged?.Invoke("Orange", true);
        StoryManager.OnCubePlaced?.Invoke("Orange", cubeName);
    
        Debug.Log($"Event OnCubePlaced envoyé: Socket=Orange, Cube='{cubeName}'");
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        if (_hapticManager != null)
        {
            _hapticManager.TriggerBothHands(0.3f, 0.1f);
        }

        Debug.Log("Socket Orange vide");
        StoryManager.OnSocketStateChanged?.Invoke("Orange", false);
        StoryManager.OnCubePlaced?.Invoke("Orange", null);
    }
}
