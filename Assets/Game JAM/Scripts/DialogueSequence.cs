using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI dialogueTextUI;

    [Header("Dialogue Settings")]
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

    private void Awake()
    {
        if (dialogueTextUI == null)
        {
            dialogueTextUI = GetComponent<TextMeshProUGUI>();
        }
    }

    public void StartDialogueBranch(int branch)
    {
        if (activeDialogue != null)
            StopCoroutine(activeDialogue);

        dialogueFinished = false;

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
        if (dialogueTextUI == null || lines == null || lines.Count == 0)
        {
            Debug.LogWarning("DialogueTextUI is null or lines are empty!");
            yield break;
        }

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        dialogueTextUI.enabled = true;

        foreach (string line in lines)
        {
            dialogueTextUI.text = "";

            foreach (char c in line)
            {
                dialogueTextUI.text += c;
                yield return new WaitForSeconds(typewriterDelay);
            }

            yield return new WaitForSeconds(displayTime);
        }

        dialogueTextUI.text = "";
        activeDialogue = null;
        dialogueFinished = true;

        Debug.Log("Dialogue terminé !");
    }
}
