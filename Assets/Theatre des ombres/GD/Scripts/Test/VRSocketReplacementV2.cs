using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class VRSocketReplacementV2 : MonoBehaviour
{
    [Header("XR References")] 
    [SerializeField] private XRSocketInteractor socketInteractor;

    [Header("Spawn Settings")] 
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private bool removeParentObject = true;
    
    [Header("Timings")] 
    [SerializeField] private float destroyDelay = DEFAULT_DESTROY_DELAY;

    private const float DEFAULT_DESTROY_DELAY = 0.05f;
    
    private IXRSelectInteractor currentHandHolding;
    private XRGrabInteractable parentGrabInteractable;
    private static XRInteractionManager cachedInteractionManager;
    
    private Vector3 savedHandLocalPosition;
    private Quaternion savedHandLocalRotation;

    private void Awake()
    {
        if (cachedInteractionManager == null)
        {
            cachedInteractionManager = FindAnyObjectByType<XRInteractionManager>();
        }
        
        parentGrabInteractable = GetComponentInParent<XRGrabInteractable>();
        if (parentGrabInteractable == null)
        {
            Debug.LogWarning("Aucun XRGrabInteractable trouvé sur le parent de ce socket!", this);
        }
    }

    private void OnEnable()
    {
        if (socketInteractor == null)
        {
            Debug.LogError("XRSocketInteractor is not assigned!", this);
            return;
        }

        if (parentGrabInteractable != null)
        {
            parentGrabInteractable.selectEntered.AddListener(OnParentGrabbed);
            parentGrabInteractable.selectExited.AddListener(OnParentReleased);
        }

        socketInteractor.selectEntered.AddListener(OnObjectInserted);
    }

    private void OnDisable()
    {
        if (parentGrabInteractable != null)
        {
            parentGrabInteractable.selectEntered.RemoveListener(OnParentGrabbed);
            parentGrabInteractable.selectExited.RemoveListener(OnParentReleased);
        }
        
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnObjectInserted);
        }
    }

    private void OnParentGrabbed(SelectEnterEventArgs args)
    {
        currentHandHolding = args.interactorObject as IXRSelectInteractor;
        
        if (currentHandHolding != null && parentGrabInteractable != null)
        {
            Transform handTransform = currentHandHolding.GetAttachTransform(parentGrabInteractable);
            if (handTransform == null)
            {
                handTransform = currentHandHolding.transform;
            }
            
            savedHandLocalPosition = parentGrabInteractable.transform.InverseTransformPoint(handTransform.position);
            savedHandLocalRotation = Quaternion.Inverse(parentGrabInteractable.transform.rotation) * handTransform.rotation;
            
            Debug.Log($"Figurine prise en main: {args.interactorObject.transform.name}");
        }
    }

    private void OnParentReleased(SelectExitEventArgs args)
    {
        Debug.Log($"Figurine relâchée de: {args.interactorObject.transform.name}");
    }

   private void OnObjectInserted(SelectEnterEventArgs args)
{
    Debug.Log($"Objet inséré dans le socket, main actuelle: {(currentHandHolding != null ? currentHandHolding.transform.name : "aucune")}");
    
    bool isParentBeingHeld = false;
    if (parentGrabInteractable != null)
    {
        isParentBeingHeld = parentGrabInteractable.isSelected;
    }
    
    Debug.Log($"La figurine parent est-elle tenue? {isParentBeingHeld}");
    
    Vector3 savedWorldPosition = Vector3.zero;
    Quaternion savedWorldRotation = Quaternion.identity;
    
    if (parentGrabInteractable != null && parentGrabInteractable.transform != null)
    {
        savedWorldPosition = parentGrabInteractable.transform.position;
        savedWorldRotation = parentGrabInteractable.transform.rotation;
    }
    
    GameObject insertedObject = null;
    if (args.interactableObject != null)
    {
        insertedObject = args.interactableObject.transform.gameObject;
    }

    GameObject spawnedObject = null;
    bool shouldDestroy = false;
    
    if (currentHandHolding != null && isParentBeingHeld)
    {
        spawnedObject = SpawnPrefab(savedWorldPosition, savedWorldRotation);
        
        if (spawnedObject != null)
        {
            shouldDestroy = true;
            
            if (cachedInteractionManager != null)
            {
                cachedInteractionManager.StartCoroutine(
                    ReattachToHandCoroutine(
                        spawnedObject, 
                        currentHandHolding, 
                        savedWorldPosition, 
                        savedWorldRotation,
                        savedHandLocalPosition,
                        savedHandLocalRotation
                    )
                );
            }
            else
            {
                Debug.LogError("XRInteractionManager introuvable pour la coroutine!");
            }
        }
        else
        {
            Debug.LogError("Échec du spawn du prefab!");
        }
    }
    else
    {
        Debug.LogWarning("Aucune main ne tient la figurine, spawn du prefab sans réattachement");
        spawnedObject = SpawnPrefab(savedWorldPosition, savedWorldRotation);
        shouldDestroy = (spawnedObject != null);
    }

    if (shouldDestroy)
    {
        if (insertedObject != null)
        {
            insertedObject.SetActive(false);
            Destroy(insertedObject, destroyDelay);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, destroyDelay);

        if (removeParentObject && transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject, destroyDelay);
        }
    }
    else
    {
        Debug.LogError("Le prefab n'a pas pu être spawné, conservation des objets!");
    }
}


    private GameObject SpawnPrefab(Vector3 position, Quaternion rotation)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Aucun prefab à spawn!", this);
            return null;
        }
        
        GameObject spawned = Instantiate(prefabToSpawn, position, rotation);
        spawned.name = prefabToSpawn.name;
        
        ObjectResetter resetter = spawned.GetComponent<ObjectResetter>();
        if (resetter == null)
        {
            resetter = spawned.AddComponent<ObjectResetter>();
        }
        resetter.SaveInitialTransform();
        
        Debug.Log($"Prefab spawné à {position}: {spawned.name}");
        return spawned;
    }
    
    private static IEnumerator ReattachToHandCoroutine(
        GameObject spawnedObject, 
        IXRSelectInteractor handInteractor, 
        Vector3 worldPosition, 
        Quaternion worldRotation,
        Vector3 handLocalPosition,
        Quaternion handLocalRotation)
    {
        yield return null;
        
        if (spawnedObject == null)
        {
            Debug.LogWarning("L'objet spawn a été détruit!");
            yield break;
        }
        
        if (handInteractor == null)
        {
            Debug.LogWarning("La main mémorisée est null!");
            yield break;
        }
            
        XRGrabInteractable grabInteractable = spawnedObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogWarning($"Le prefab '{spawnedObject.name}' n'a pas de XRGrabInteractable!");
            yield break;
        }
        
        if (cachedInteractionManager == null)
        {
            Debug.LogError("XRInteractionManager introuvable!");
            yield break;
        }
        
        if (handInteractor is XRBaseInteractor baseInteractor)
        {
            GameObject attachPoint = new GameObject("DynamicAttachPoint");
            attachPoint.transform.SetParent(spawnedObject.transform, false);
            attachPoint.transform.localPosition = handLocalPosition;
            attachPoint.transform.localRotation = handLocalRotation;
            
            Transform originalAttachTransform = grabInteractable.attachTransform;
            bool originalMatchPosition = grabInteractable.matchAttachPosition;
            bool originalMatchRotation = grabInteractable.matchAttachRotation;
            bool originalTrackRotation = grabInteractable.trackRotation;
            
            grabInteractable.attachTransform = attachPoint.transform;
            grabInteractable.matchAttachPosition = false;
            grabInteractable.matchAttachRotation = false;
            grabInteractable.trackRotation = false;
            
            yield return null;
            
            IXRSelectInteractable selectInteractable = grabInteractable;
            
            bool canSelect = baseInteractor.CanSelect(selectInteractable);
            bool isSelecting = baseInteractor.IsSelecting(selectInteractable);
            
            Debug.Log($"Tentative réattachement - CanSelect: {canSelect}, IsSelecting: {isSelecting}");
            
            if (canSelect && !isSelecting)
            {
                cachedInteractionManager.SelectEnter(baseInteractor, selectInteractable);
                Debug.Log($"✓ Assemblage réattaché à {baseInteractor.name}");
                
                cachedInteractionManager.StartCoroutine(KeepRotationFixed(spawnedObject.transform, worldRotation, grabInteractable, originalAttachTransform, originalMatchPosition, originalMatchRotation, originalTrackRotation, attachPoint));
            }
            else
            {
                Debug.LogWarning($"✗ Impossible de réattacher - CanSelect: {canSelect}, IsSelecting: {isSelecting}");
                
                grabInteractable.attachTransform = originalAttachTransform;
                grabInteractable.matchAttachPosition = originalMatchPosition;
                grabInteractable.matchAttachRotation = originalMatchRotation;
                grabInteractable.trackRotation = originalTrackRotation;
                
                Destroy(attachPoint);
            }
        }
    }
    
    private static IEnumerator KeepRotationFixed(Transform objectTransform, Quaternion targetRotation, XRGrabInteractable grabInteractable, Transform originalAttach, bool originalMatchPos, bool originalMatchRot, bool originalTrackRot, GameObject attachPoint)
    {
        float elapsed = 0f;
        while (elapsed < 0.2f)
        {
            if (objectTransform != null)
            {
                objectTransform.rotation = targetRotation;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        if (grabInteractable != null)
        {
            grabInteractable.attachTransform = originalAttach;
            grabInteractable.matchAttachPosition = originalMatchPos;
            grabInteractable.matchAttachRotation = originalMatchRot;
            grabInteractable.trackRotation = originalTrackRot;
        }
        
        if (attachPoint != null)
        {
            Destroy(attachPoint);
        }
    }
}
