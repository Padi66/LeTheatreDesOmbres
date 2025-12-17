using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PiedestalUP : MonoBehaviour
{
    public Light _lightGreen;
    public Light _lightOrange;
    public Light _lightPurple;
    public float _lightTransitionDuration = 1f;
    
    public XRLockSocketInteractor _socketGreen;
    public XRLockSocketInteractor _socketOrange;
    public XRLockSocketInteractor _socketPurple;
    public DialogueSequence _dialogueSequence;
    public float _duration = 2f;
    
    [Header("Dialogue Delays")]
    [Tooltip("Délai avant de jouer le nom de la figurine (Branch 5/8) pour que le choix passe d'abord")]
    public float figurineNameDelay = 0.3f;
    
    [SerializeField] private GameObject _socketOrangeVisual;
    [SerializeField] private GameObject _socketGreenVisual;
    [SerializeField] private GameObject _socketPurpleVisual;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] private AudioClip Fixe2;
    [SerializeField] private AudioClip Fixe3;
    [SerializeField] private GameObject Machine;

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
        
        StartCoroutine(DelayedDialogueBranch(5, figurineNameDelay));
        
        SetLayerRecursively(_socketGreenVisual, 0);
        StartCoroutine(TurnOnLight(_lightOrange, socket));
        SetLayerRecursively(_socketOrangeVisual, 6);
        Machine.layer = 6;
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
        
        StartCoroutine(DelayedDialogueBranch(8, figurineNameDelay));
        
        SetLayerRecursively(_socketOrangeVisual, 0);
        Machine.layer = 0;
        StartCoroutine(TurnOnLight(_lightPurple, socket));
        SetLayerRecursively(_socketPurpleVisual, 6);
    }
    
    IEnumerator DelayedDialogueBranch(int branchNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        _dialogueSequence.StartDialogueBranch(branchNumber);
        Debug.Log($"Branch {branchNumber} lancée après délai de {delay}s");
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
    
    IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
