using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlaySoundOnSocketAttach : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip attachSound;
    
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.8f;
    
    private XRSocketInteractor socketInteractor;
    private AudioSource audioSource;

    void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
    }

    void OnEnable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnSocketAttach);
        }
    }

    void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSocketAttach);
        }
    }

    private void OnSocketAttach(SelectEnterEventArgs args)
    {
        if (attachSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attachSound, volume);
        }
    }
    
}
