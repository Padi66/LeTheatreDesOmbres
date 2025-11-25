using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportationEvent : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public GameObject dialogueText;
    public float typewriterDelay = 0.05f;

    [Header("Blocage TP")]
    public TeleportationAnchor teleportAnchor;

    [Header("État joueur")]
    public bool isPlayerInPublic = false;

    private TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        if (dialogueText != null)
        {
            textMeshProUGUI = dialogueText.GetComponentInChildren<TextMeshProUGUI>();
            //dialogueText.SetActive(false);
        }

        if (teleportAnchor == null)
            teleportAnchor = GetComponent<TeleportationAnchor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPublic = true;
            Debug.Log("Le joueur est entré dans la zone du Teleport Anchor.");

            if (teleportAnchor != null)
                teleportAnchor.enabled = false;

            //if (dialogueText != null)
                //dialogueText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPublic = false;
            Debug.Log("Le joueur a quitté la zone du Teleport Anchor.");

            //if (dialogueText != null)
                //dialogueText.SetActive(false);
        }
    }

    public IEnumerator ShowTextTypewriter(string fullText)
    {
        if (textMeshProUGUI == null) yield break;

        textMeshProUGUI.text = "";
        foreach (char c in fullText)
        {
            textMeshProUGUI.text += c;
            yield return new WaitForSeconds(typewriterDelay);
        }
    }

    public void EnableTeleport()
    {
        if (teleportAnchor != null)
            teleportAnchor.enabled = true;
    }
}
