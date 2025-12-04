using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OutlineOnInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool outlineOnHover = true;
    [SerializeField] private bool outlineOnGrab = false;
    
    private XRGrabInteractable grabInteractable;
    private int outlineLayer;
    private int originalLayer;
    
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        outlineLayer = LayerMask.NameToLayer("Outline");
        originalLayer = gameObject.layer;
    }
    
    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            if (outlineOnHover)
            {
                grabInteractable.hoverEntered.AddListener(OnHoverEntered);
                grabInteractable.hoverExited.AddListener(OnHoverExited);
            }
            
            if (outlineOnGrab)
            {
                grabInteractable.selectEntered.AddListener(OnGrabbed);
                grabInteractable.selectExited.AddListener(OnReleased);
            }
        }
    }
    
    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            if (outlineOnHover)
            {
                grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
                grabInteractable.hoverExited.RemoveListener(OnHoverExited);
            }
            
            if (outlineOnGrab)
            {
                grabInteractable.selectEntered.RemoveListener(OnGrabbed);
                grabInteractable.selectExited.RemoveListener(OnReleased);
            }
        }
    }
    
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        gameObject.layer = outlineLayer;
    }
    
    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (!grabInteractable.isSelected)
        {
            gameObject.layer = originalLayer;
        }
    }
    
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        gameObject.layer = outlineLayer;
    }
    
    private void OnReleased(SelectExitEventArgs args)
    {
        gameObject.layer = originalLayer;
    }
}
