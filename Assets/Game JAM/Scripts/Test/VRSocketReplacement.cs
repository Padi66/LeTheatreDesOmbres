using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
/// <summary>
/// This component listens to a XR Socket Interactor.  
/// When an object is inserted in the socket, it destroys:
/// - The socket itself
/// - The socket parent (optional)
/// - The inserted object  
/// Then it spawns a configured prefab in their place.
/// </summary>
public class VRSocketReplacement : MonoBehaviour
{
    [Header("XR References")]
    [SerializeField] private XRSocketInteractor socketInteractor;



    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private bool removeParentObject = true;

    [Header("Timings")]
    [SerializeField] private float destroyDelay = DEFAULT_DESTROY_DELAY;

    // Constants
    private const float DEFAULT_DESTROY_DELAY = 0.05f;

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnObjectInserted);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnObjectInserted);
    }

    /// <summary>
    /// Triggered when an object is inserted into the socket.
    /// Handles destruction and prefab spawning.
    /// </summary>
    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        // Destroy the inserted object.
        if (args.interactableObject != null)
        {
            Destroy(args.interactableObject.transform.gameObject, destroyDelay);
        }

        // Spawn the replacement prefab.
        SpawnPrefab();

        // Destroy the socket and optionally its parent.
        Destroy(gameObject, destroyDelay);

        if (removeParentObject && transform.parent != null)
        {
            Destroy(transform.parent.gameObject, destroyDelay);
        }
    }

    /// <summary>
    /// Spawns the configured prefab at the socket's position and rotation.
    /// </summary>
    private void SpawnPrefab()
    {
        if (prefabToSpawn == null)
            return;

        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}
