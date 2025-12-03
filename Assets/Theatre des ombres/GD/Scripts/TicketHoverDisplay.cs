using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TicketHoverDisplay : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;
    
    private ControllerCanvasManager currentCanvasManager;
    
    private void OnEnable()
    {
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }
    
    private void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        ControllerCanvasManager canvasManager = GetControllerCanvasManager(args.interactorObject as XRBaseInteractor);
        if (canvasManager != null)
        {
            currentCanvasManager = canvasManager;
            canvasManager.ShowHoverImage();
        }
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (currentCanvasManager != null && !grabInteractable.isSelected)
        {
            currentCanvasManager.HideAllImages();
            currentCanvasManager = null;
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        ControllerCanvasManager canvasManager = GetControllerCanvasManager(args.interactorObject as XRBaseInteractor);
        if (canvasManager != null)
        {
            currentCanvasManager = canvasManager;
            canvasManager.ShowSelectImage();
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (currentCanvasManager != null)
        {
            currentCanvasManager.HideAllImages();
            currentCanvasManager = null;
        }
    }

    private ControllerCanvasManager GetControllerCanvasManager(XRBaseInteractor interactor)
    {
        if (interactor == null) return null;
        
        Transform current = interactor.transform;
        while (current != null)
        {
            ControllerCanvasManager manager = current.GetComponent<ControllerCanvasManager>();
            if (manager != null)
            {
                return manager;
            }
            current = current.parent;
        }
        
        return null;
    }
}

