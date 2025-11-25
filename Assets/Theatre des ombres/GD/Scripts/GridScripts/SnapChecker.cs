using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapChecker : MonoBehaviour
{
    public int socketID;
    private XRSocketInteractor socket;
    private GameObject currentObject;

    public string CurrentTag => currentObject != null ? currentObject.tag : "";
    public bool HasObject => currentObject != null;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnObjectPlaced);
        socket.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        currentObject = args.interactableObject.transform.gameObject;
        PuzzleManager.Instance.CheckCombination();
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        currentObject = null;
        PuzzleManager.Instance.CheckCombination();
    }
}