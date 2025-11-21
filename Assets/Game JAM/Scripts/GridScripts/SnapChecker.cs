using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapChecker: MonoBehaviour
{
    public string expectedTag; // tag du bloc attendu
    private XRSocketInteractor socket;
    private GameObject currentObject;

    public bool IsCorrect => currentObject != null && currentObject.CompareTag(expectedTag);

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnObjectPlaced);
        socket.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        currentObject = args.interactableObject.transform.gameObject;
        PuzzleManager.Instance.CheckAllSockets();
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        currentObject = null;
        PuzzleManager.Instance.CheckAllSockets();
    }
}