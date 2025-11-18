using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TeleportationEvent teleportEvent;
    public float typewriterDelay = 0.05f;
    public float displayTime = 2f;

    [Header("Dialogue Branches")]
    [TextArea(2, 5)] public List<string> dialogueBranch1;
    [TextArea(2, 5)] public List<string> dialogueBranch2;
    [TextArea(2, 5)] public List<string> dialogueBranch3;
    [TextArea(2, 5)] public List<string> dialogueBranch4;
    [TextArea(2, 5)] public List<string> dialogueBranch5;
    [TextArea(2, 5)] public List<string> dialogueBranch6;

    private Coroutine activeDialogue;

    [HideInInspector] public bool dialogueFinished = false;

    public void StartDialogueBranch(int branch)
    {
        if (activeDialogue != null)
            StopCoroutine(activeDialogue);

        dialogueFinished = false; //reset à chaque nouveau dialogue

        if (branch == 1)
            activeDialogue = StartCoroutine(ShowDialogueSequence(dialogueBranch1));
        else if (branch == 2)
            activeDialogue = StartCoroutine(ShowDialogueSequence(dialogueBranch2));
        else if (branch == 3)
            activeDialogue = StartCoroutine(ShowDialogueSequence(dialogueBranch3));
        else if (branch == 4)
            activeDialogue = StartCoroutine(ShowDialogueSequence(dialogueBranch4));
        else if (branch == 5)
            activeDialogue = StartCoroutine(ShowDialogueSequence(dialogueBranch5));
        else if (branch == 6)
            activeDialogue = StartCoroutine(ShowDialogueSequence(dialogueBranch6));
        else
            Debug.LogWarning("Numéro de branche inconnu !");
    }

    private IEnumerator ShowDialogueSequence(List<string> lines)
    {
        GetComponent<TMP_Text>().enabled = true;
        if (teleportEvent == null || lines == null || lines.Count == 0)
            yield break;

        var textUI = teleportEvent.dialogueText.GetComponentInChildren<TextMeshProUGUI>();
        teleportEvent.dialogueText.SetActive(true);

        foreach (string line in lines)
        {
            textUI.text = "";
            foreach (char c in line)
            {
                textUI.text += c;
                yield return new WaitForSeconds(typewriterDelay);
            }
            yield return new WaitForSeconds(displayTime);
        }

        teleportEvent.dialogueText.SetActive(false);
        activeDialogue = null;

        teleportEvent.EnableTeleport();

        //Dialogue terminé
        dialogueFinished = true;
        Debug.Log("Dialogue terminé !");
    }
}
