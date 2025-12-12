using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PiedestalUP : MonoBehaviour
{
    // Anciennes variables pour l'animation de position (désactivées)
    // public Transform _startPositionGreen;
    // public Transform _endPositionGreen;
    // public Transform _startPositionPurple;
    // public Transform _endPositionPurple;
    // public Transform _startPositionOrange;
    // public Transform _endPositionOrange;
    // public Transform _piedestalOrange;
    // public Transform _piedestalPurple;
    // public Transform _piedestalGreen;
    
    // Nouvelles variables pour le système de lumières
    public Light _lightGreen;
    public Light _lightOrange;
    public Light _lightPurple;
    public float _lightTransitionDuration = 1f;
    
    public XRLockSocketInteractor _socketGreen;
    public XRLockSocketInteractor _socketOrange;
    public XRLockSocketInteractor _socketPurple;
    public DialogueSequence _dialogueSequence;
    public float _duration = 2f;
    
    [SerializeField] private GameObject _socketOrangeVisual;
    [SerializeField] private GameObject _socketGreenVisual;
    [SerializeField] private GameObject _socketPurpleVisual;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] private AudioClip Fixe2;
    [SerializeField] private AudioClip Fixe3;
    
    
    


    private void Start()
    {
        StartCoroutine(Delay(6));
        _dialogueSequence.StartDialogueBranch(0);
        _dialogueSequence.StartDialogueBranch(1);
        UpGreen(_socketGreen);
        
    }

    public void UpOrange(XRLockSocketInteractor socketToReactivate = null)
    {
        
       
        XRLockSocketInteractor socket = socketToReactivate ?? _socketOrange;
        _dialogueSequence.StartDialogueBranch(5);
        _audioSource.Stop();
        _audioSource.PlayOneShot(Fixe2);
        SetLayerRecursively(_socketGreenVisual, 0);
        StartCoroutine(TurnOnLight(_lightOrange, socket));
        SetLayerRecursively(_socketOrangeVisual, 6);
        
    }

    public void UpGreen(XRLockSocketInteractor socketToReactivate = null)
    {
        XRLockSocketInteractor socket = socketToReactivate ?? _socketGreen;
        StartCoroutine(TurnOnLight(_lightGreen, socket));
        SetLayerRecursively(_socketGreenVisual, 6);
    }

    public void UpPurple(XRLockSocketInteractor socketToReactivate = null)
    {
        
        
        XRLockSocketInteractor socket = socketToReactivate ?? _socketPurple;
        _dialogueSequence.StartDialogueBranch(8);
        _audioSource.Stop();
        _audioSource.PlayOneShot(Fixe3);
        SetLayerRecursively(_socketOrangeVisual, 0);
        StartCoroutine(TurnOnLight(_lightPurple, socket));
        SetLayerRecursively(_socketPurpleVisual, 6);
    }
    
    IEnumerator TurnOnLight(Light light, XRLockSocketInteractor socketToReactivate = null)
    {
        if (light == null)
        {
            Debug.LogWarning("Light is null, cannot turn on");
            yield break;
        }

        light.enabled = true;
        float elapsed = 0f;
        float startIntensity = 0f;
        float targetIntensity = 50f;

        while (elapsed < _lightTransitionDuration)
        {
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / _lightTransitionDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log($"Socket {socketToReactivate.name} réactivé après l'allumage de la lumière");
        }
    }

    IEnumerator TurnOffLight(Light light)
    {
        if (light == null || !light.enabled)
        {
            yield break;
        }

        float elapsed = 0f;
        float startIntensity = light.intensity;
        float targetIntensity = 0f;

        while (elapsed < _lightTransitionDuration)
        {
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / _lightTransitionDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        light.intensity = 0f;
        light.enabled = false;
    }
    
  
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    // Anciennes coroutines pour l'animation de montée (désactivées)
    /*
    IEnumerator UpEnumGreen(XRLockSocketInteractor socketToReactivate = null)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalGreen.position = Vector3.Lerp(_startPositionGreen.position, _endPositionGreen.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalGreen.position = _endPositionGreen.position;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log("Socket Green réactivé après la montée du piédestal");
        }
    }

    IEnumerator DownEnumGreen()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalGreen.position = Vector3.Lerp(_endPositionGreen.position, _startPositionGreen.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalGreen.position = _startPositionGreen.position;
    }
    
    IEnumerator UpEnumPurple(XRLockSocketInteractor socketToReactivate = null)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalPurple.position = Vector3.Lerp(_startPositionPurple.position, _endPositionPurple.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalPurple.position = _endPositionPurple.position;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log("Socket Purple réactivé après la montée du piédestal");
        }
    }
    
    IEnumerator DownEnumPurple()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalPurple.position = Vector3.Lerp(_endPositionPurple.position, _startPositionPurple.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _piedestalPurple.position = _startPositionPurple.position;
    }
    
    IEnumerator UpEnumOrange(XRLockSocketInteractor socketToReactivate = null)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalOrange.position = Vector3.Lerp(_startPositionOrange.position, _endPositionOrange.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    
        _piedestalOrange.position = _endPositionOrange.position;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log("Socket Orange réactivé après la montée du piédestal");
        }
    }

    IEnumerator DownEnumOrange()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalOrange.position = Vector3.Lerp(_endPositionOrange.position, _startPositionOrange.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _piedestalOrange.position = _startPositionOrange.position;
    }
    */
    
    IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
