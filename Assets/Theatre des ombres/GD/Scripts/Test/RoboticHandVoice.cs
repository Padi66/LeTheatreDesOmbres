using UnityEngine;

public class RoboticHandVoice : MonoBehaviour
{
    [Header("Audio Settings")] public AudioSource audioSource;

    [Header("Voice Clips")] [Tooltip("Son joué quand le joueur prend le billet 'Commencer'")]
    public AudioClip onPlayTicketGrabbed;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayerGrabbedPlayTicket()
    {
        if (audioSource != null && onPlayTicketGrabbed != null)
        {
            audioSource.PlayOneShot(onPlayTicketGrabbed);
        }
    }
}