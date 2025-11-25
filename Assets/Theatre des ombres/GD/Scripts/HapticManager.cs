using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;

public class HapticManager : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private bool _testOnStart = true;
    [SerializeField] private float _testAmplitude = 0.7f;
    [SerializeField] private float _testDuration = 0.3f;

    

    private IEnumerator TestBothHands()
    {
        yield return new WaitForSeconds(1f);
        TriggerLeftHand(0.8f, 0.2f);
        yield return new WaitForSeconds(0.5f);
        TriggerRightHand(0.8f, 0.2f);
    }

    public void TriggerLeftHand(float amplitude, float duration)
    {
        StartCoroutine(SendHapticToHand(amplitude, duration, InputDeviceCharacteristics.Left));
    }

    public void TriggerRightHand(float amplitude, float duration)
    {
        StartCoroutine(SendHapticToHand(amplitude, duration, InputDeviceCharacteristics.Right));
    }

    public void TriggerBothHands(float amplitude, float duration)
    {
        StartCoroutine(TestHapticFeedback(amplitude, duration, 0f));
    }

    private IEnumerator SendHapticToHand(float amplitude, float duration, InputDeviceCharacteristics handSide)
    {
        yield return new WaitForSeconds(0.1f);

        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics characteristics = handSide | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

        string handName = (handSide == InputDeviceCharacteristics.Left) ? "Left" : "Right";
        Debug.Log($"Searching for {handName} hand controller... Found {devices.Count} device(s)");

        foreach (InputDevice device in devices)
        {
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    bool success = device.SendHapticImpulse(channel, amplitude, duration);
                    Debug.Log($"✅ Haptic sent to {handName} hand ({device.name}): amplitude={amplitude}, duration={duration}, success={success}");
                }
                else
                {
                    Debug.LogWarning($"❌ {handName} hand device does not support haptic impulse");
                }
            }
        }

        if (devices.Count == 0)
        {
            Debug.LogWarning($"❌ No {handName} hand controller found!");
        }
    }

    private IEnumerator TestHapticFeedback(float _amplitude, float _duration, float _frequency)
    {
        yield return new WaitForSeconds(0.1f);

        List<InputDevice> rightDevices = new List<InputDevice>();
        List<InputDevice> leftDevices = new List<InputDevice>();

        InputDeviceCharacteristics rightHandedCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDeviceCharacteristics leftHandedCharacteristics =
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightHandedCharacteristics, rightDevices);
        InputDevices.GetDevicesWithCharacteristics(leftHandedCharacteristics, leftDevices);

        List<InputDevice> allDevices = new List<InputDevice>();
        allDevices.AddRange(rightDevices);
        allDevices.AddRange(leftDevices);

        Debug.Log($"Found {allDevices.Count} controllers (Right: {rightDevices.Count}, Left: {leftDevices.Count})");

        foreach (InputDevice device in allDevices)
        {
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    bool success = device.SendHapticImpulse(channel, _amplitude, _duration);
                    Debug.Log($"✅ Haptic sent to {device.name}: amplitude={_amplitude}, duration={_duration}, success={success}");
                }
                else
                {
                    Debug.LogWarning($"❌ Device {device.name} does not support haptic impulse");
                }
            }
        }
    }
}