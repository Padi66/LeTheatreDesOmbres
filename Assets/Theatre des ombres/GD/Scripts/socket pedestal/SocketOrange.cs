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
    private bool _hasDone = false;

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

        if (cube.GetComponent<Sword>())
        {
            _particleSystem.Play();
            Debug.Log("Socket Orange contient Sword");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpPurple(_socketInteractor);
                    _hasDone = true;
                }
            }
            _storyManager.CheckDirectStep2();
        }
        else if (cube.GetComponent<Shield>())
        {
            _particleSystem.Play();
            Debug.Log("Socket Orange contient Shield");
            if (_piedestal != null)
            {
                if (!_hasDone)
                {
                    _piedestal.UpPurple(_socketInteractor);
                    _hasDone = true;
                }
            }
            _storyManager.CheckDirectStep2();
        }
        StoryManager.OnSocketStateChanged?.Invoke("Orange", true);
        StoryManager.OnCubePlaced?.Invoke("Orange", _cubeName);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
       

        Debug.Log("Socket Orange vide");
        StoryManager.OnSocketStateChanged?.Invoke("Orange", false);
        StoryManager.OnCubePlaced?.Invoke("Orange", null);
    }
    
}
