using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlayTicket : MonoBehaviour
{
    [Header("References")]
    public RoboticHandVoice roboticHand;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnTicketGrabbed);
        }

        if (roboticHand == null)
        {
            roboticHand = FindFirstObjectByType<RoboticHandVoice>();
        }
    }

    void OnTicketGrabbed(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (roboticHand != null)
        {
            roboticHand.OnPlayerGrabbedPlayTicket();
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnTicketGrabbed);
        }
    }
    
}