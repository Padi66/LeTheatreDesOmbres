using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketOrange : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor _socketInteractor;
    [SerializeField] private PiedestalUP _piedestal;
    [SerializeField] private float _lockDelay = 0.3f;
    [SerializeField] private StoryManager _storyManager;

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
            Debug.Log("Socket Orange contient Cube Vert");
            if (_piedestal != null)
            {
                _piedestal.UpOrange();
            }
            _storyManager.CheckDirectStep2();
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            Debug.Log("Socket Orange contient Cube Orange");
            if (_piedestal != null)
            {
                _piedestal.UpOrange();
            }
            _storyManager.CheckDirectStep2();
        }
        else if (cube.GetComponent<CubePurple>())
        {
            Debug.Log("Socket Orange contient Cube Violet");
            if (_piedestal != null)
            {
                _piedestal.UpOrange();
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
