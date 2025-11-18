using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportationZone : MonoBehaviour
{
    [Header("XR Move Provider à contrôler")]
    public ContinuousMoveProvider moveProvider;

    [Header("Désactiver le déplacement ici ?")]
    public bool disableMovementOnArrival = true;

    private TeleportationAnchor teleportAnchor;

    void Awake()
    {
        teleportAnchor = GetComponent<TeleportationAnchor>();
    }

    void OnEnable()
    {
        if (teleportAnchor != null)
            teleportAnchor.teleporting.AddListener(OnTeleported);
    }

    void OnDisable()
    {
        if (teleportAnchor != null)
            teleportAnchor.teleporting.RemoveListener(OnTeleported);
    }

    private void OnTeleported(TeleportingEventArgs args)
    {
        if (moveProvider == null)
        {
            Debug.LogWarning("Aucun Move Provider assigné !");
            return;
        }

        if (disableMovementOnArrival)
        {
            moveProvider.enabled = false;
            Debug.Log("Mouvement désactivé après téléportation sur " + gameObject.name);
        }
        else
        {
            moveProvider.enabled = true;
            Debug.Log("Mouvement activé après téléportation sur " + gameObject.name);
        }
    }
}
