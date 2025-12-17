using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ButtonHoverDisplay : MonoBehaviour
{
    [SerializeField] private XRPushButton buttonInteractable;
    private ControllerCanvasManager currentCanvasManager;
    private XRBaseInteractor currentInteractor;
    
    private void OnEnable()
    {
        
        buttonInteractable.hoverEntered.AddListener(OnHoverEnteredButton);
        buttonInteractable.hoverExited.AddListener(OnHoverExitedButton);
    }
    
    private void OnDisable()
    {
        buttonInteractable.hoverEntered.RemoveListener(OnHoverEnteredButton);
        buttonInteractable.hoverExited.RemoveListener(OnHoverExitedButton);
    }
    private void OnHoverEnteredButton(HoverEnterEventArgs args)
    {
        if (currentCanvasManager != null)
        {
            return;
        }

        ControllerCanvasManager canvasManager = GetControllerCanvasManager(args.interactorObject as XRBaseInteractor);
        if (canvasManager != null)
        {
            currentInteractor = args.interactorObject as XRBaseInteractor;
            currentCanvasManager = canvasManager;
            canvasManager.ShowButtonImage();
        }
    }

    private void OnHoverExitedButton(HoverExitEventArgs args)
    {
        if (currentInteractor == args.interactorObject && currentCanvasManager != null && !buttonInteractable.isSelected)
        {
            currentCanvasManager.HideAllImages();
            currentCanvasManager = null;
            currentInteractor = null;
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
