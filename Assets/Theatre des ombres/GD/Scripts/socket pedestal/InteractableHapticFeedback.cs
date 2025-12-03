using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InteractableHapticFeedback : MonoBehaviour
{
    [Header("Haptic Settings")]
    [SerializeField] private float _grabAmplitude = 0.4f;
    [SerializeField] private float _grabDuration = 0.15f;
    [SerializeField] private float _releaseAmplitude = 0.2f;
    [SerializeField] private float _releaseDuration = 0.1f;
    
    private XRGrabInteractable _grabInteractable;
    
    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.AddListener(OnGrabbed);
            _grabInteractable.selectExited.AddListener(OnReleased);
            Debug.Log($"[HAPTIC] InteractableHapticFeedback activé sur {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"[HAPTIC] Pas de XRGrabInteractable trouvé sur {gameObject.name}");
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
        
        SendHapticToHand(isLeftHand, _grabAmplitude, _grabDuration);
    }
    
    void OnReleased(SelectExitEventArgs args)
    {
        bool isLeftHand = args.interactorObject.transform.name.Contains("Left");
        string handName = isLeftHand ? "GAUCHE" : "DROITE";
        
        Debug.Log($"[HAPTIC] {gameObject.name} relâché par la main {handName}");
        
        SendHapticToHand(isLeftHand, _releaseAmplitude, _releaseDuration);
    }
    
    void SendHapticToHand(bool isLeftHand, float amplitude, float duration)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceRole role = isLeftHand ? InputDeviceRole.LeftHanded : InputDeviceRole.RightHanded;
        
        InputDevices.GetDevicesWithRole(role, devices);
        
        foreach (var device in devices)
        {
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    device.SendHapticImpulse(channel, amplitude, duration);
                    Debug.Log($"[HAPTIC] ✅ Vibration envoyée à la main {(isLeftHand ? "GAUCHE" : "DROITE")}: amplitude={amplitude}, duration={duration}");
                }
                else
                {
                    Debug.LogWarning($"[HAPTIC] Le contrôleur {(isLeftHand ? "gauche" : "droit")} ne supporte pas les haptics");
                }
            }
        }
        
        if (devices.Count == 0)
        {
            Debug.LogWarning($"[HAPTIC] Aucun contrôleur {(isLeftHand ? "gauche" : "droit")} trouvé");
        }
    }
}
