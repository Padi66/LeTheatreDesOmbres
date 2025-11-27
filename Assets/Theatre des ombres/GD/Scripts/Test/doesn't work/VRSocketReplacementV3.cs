using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections; 
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class VRSocketReplacementV3 : MonoBehaviour
{
    [Header("XR References")]
    [SerializeField] private XRSocketInteractor socketInteractor;

    [Header("Hand Interactors (Near-Far)")]
    [SerializeField] private XRBaseInteractor leftHandInteractor;
    [SerializeField] private XRBaseInteractor rightHandInteractor;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;

    [SerializeField] private bool removeParentObject = true;

    [Header("Timings")]
    [SerializeField] private float destroyDelay = DEFAULT_DESTROY_DELAY;

    private const float DEFAULT_DESTROY_DELAY = 0.1f;

    private void OnEnable()
    {
        if (socketInteractor == null)
        {
            Debug.LogError("XRSocketInteractor is not assigned!", this);
            return;
        }

        socketInteractor.selectEntered.AddListener(OnObjectInserted);
    }

    private void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnObjectInserted);
        }
    }

    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        // Determine which hand is holding the parent of this socket
        XRBaseInteractor grabbingHand = GetHandHoldingParent();

        // Disable and destroy the inserted object
        if (args.interactableObject != null)
        {
            GameObject insertedObject = args.interactableObject.transform.gameObject;
            insertedObject.SetActive(false);
            Destroy(insertedObject, destroyDelay);
        }

        // Spawn the replacement prefab
        GameObject spawned = SpawnPrefab();

        if (spawned != null && grabbingHand != null)
            GrabWithHand(spawned, grabbingHand);

        // Start coroutine to safely destroy this object and optionally its parent
        StartCoroutine(DestroySelfAndParentCoroutine());
    }

    private XRBaseInteractor GetHandHoldingParent()
    {
        Transform currentParent = transform.parent;
        if (currentParent == null)
            return null;

        // Check left hand
        if (leftHandInteractor != null && leftHandInteractor.interactablesSelected.Count > 0)
        {
            var selected = leftHandInteractor.GetOldestInteractableSelected()?.transform;
            if (selected != null && currentParent.IsChildOf(selected))
                return leftHandInteractor;
        }

        // Check right hand
        if (rightHandInteractor != null && rightHandInteractor.interactablesSelected.Count > 0)
        {
            var selected = rightHandInteractor.GetOldestInteractableSelected()?.transform;
            if (selected != null && currentParent.IsChildOf(selected))
                return rightHandInteractor;
        }

        return null; // No hand is holding the parent
    }

    private GameObject SpawnPrefab()
    {
        if (prefabToSpawn == null)
            return null;

        return Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }

    private void GrabWithHand(GameObject obj, XRBaseInteractor hand)
    {
        XRGrabInteractable interactable = obj.GetComponent<XRGrabInteractable>();
        if (interactable == null)
        {
            Debug.LogError("Spawned prefab has no XRGrabInteractable!", obj);
            return;
        }

        // Release any current manual interaction safely
        if (hand.isPerformingManualInteraction)
            hand.EndManualInteraction();

        // Force grab
        hand.StartManualInteraction(interactable as IXRSelectInteractable);

        // Release manual mode in next frame safely
        StartCoroutine(ReenableUserReleaseSafe(hand));
    }

    private IEnumerator ReenableUserReleaseSafe(XRBaseInteractor hand)
    {
        yield return new WaitForSeconds(0.02f);

        if (hand.isPerformingManualInteraction)
            hand.EndManualInteraction();
    }

    private IEnumerator DestroySelfAndParentCoroutine()
    {
        yield return new WaitForSeconds(destroyDelay);

        if (removeParentObject && transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
