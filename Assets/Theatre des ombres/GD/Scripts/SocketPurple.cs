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
    [SerializeField] private StoryManager _storyManager;

    public bool _isInSocket = false;

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

        if (cube.GetComponent<CubeGreen>())
        {
            _isInSocket = true;
            cubeName = "CubeGreen";
            Debug.Log($"Socket Violet contient Cube Vert - nom envoyé: '{cubeName}'");
            _storyManager.CheckDirectStep3();
        }
        else if (cube.GetComponent<CubeOrange>())
        {
            _isInSocket = true;
            cubeName = "CubeOrange";
            Debug.Log($"Socket Violet contient Cube Orange - nom envoyé: '{cubeName}'");
            _storyManager.CheckDirectStep3();
        }
        else if (cube.GetComponent<CubePurple>())
        {
            _isInSocket = true;
            cubeName = "CubePurple";
            Debug.Log($"Socket Violet contient Cube Violet - nom envoyé: '{cubeName}'");
            _storyManager.CheckDirectStep3();
        }

        StoryManager.OnSocketStateChanged?.Invoke("Purple", true);
        StoryManager.OnCubePlaced?.Invoke("Purple", cubeName);

        Debug.Log($"Event OnCubePlaced envoyé: Socket=Purple, Cube='{cubeName}'");
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        _isInSocket = false;
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
