using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractableHapticFeedback : MonoBehaviour
{
    [Header("Haptic Settings")]
    [SerializeField] private HapticManager _hapticManager;
    [SerializeField] private float _grabAmplitude = 0.4f;
    [SerializeField] private float _grabDuration = 0.15f;
    [SerializeField] private float _releaseAmplitude = 0.2f;
    [SerializeField] private float _releaseDuration = 0.1f;
    
    private XRGrabInteractable _grabInteractable;
    
    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (_hapticManager == null)
        {
            _hapticManager = FindFirstObjectByType<HapticManager>();
        }
        
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.AddListener(OnGrabbed);
            _grabInteractable.selectExited.AddListener(OnReleased);
            Debug.Log($"InteractableHapticFeedback activé sur {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Pas de XRGrabInteractable trouvé sur {gameObject.name}");
        }
    }
    
    void OnDestroy()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            _grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
    
    void OnGrabbed(SelectEnterEventArgs args)
    {
        bool isLeftHand = args.interactorObject.transform.name.Contains("Left");
        string handName = isLeftHand ? "GAUCHE" : "DROITE";
        
        Debug.Log($"[HAPTIC] {gameObject.name} saisi avec la main {handName}");
        
        if (_hapticManager != null)
        {
            if (isLeftHand)
                _hapticManager.TriggerLeftHand(_grabAmplitude, _grabDuration);
            else
                _hapticManager.TriggerRightHand(_grabAmplitude, _grabDuration);
        }
    }
    
    void OnReleased(SelectExitEventArgs args)
    {
        bool isLeftHand = args.interactorObject.transform.name.Contains("Left");
        string handName = isLeftHand ? "GAUCHE" : "DROITE";
        
        Debug.Log($"[HAPTIC] {gameObject.name} relâché par la main {handName}");
        
        if (_hapticManager != null)
        {
            if (isLeftHand)
                _hapticManager.TriggerLeftHand(_releaseAmplitude, _releaseDuration);
            else
                _hapticManager.TriggerRightHand(_releaseAmplitude, _releaseDuration);
        }
    }
}
