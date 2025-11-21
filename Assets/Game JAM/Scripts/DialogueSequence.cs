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
    [TextArea(2, 5)] public List<string> branch0;
    [TextArea(2, 5)] public List<string> branch1;
    [TextArea(2, 5)] public List<string> branch2;
    [TextArea(2, 5)] public List<string> branch3;
    [TextArea(2, 5)] public List<string> branch4;
    [TextArea(2, 5)] public List<string> branch5;
    [TextArea(2, 5)] public List<string> branch6;
    [TextArea(2, 5)] public List<string> branch7;
    [TextArea(2, 5)] public List<string> branch8;
    [TextArea(2, 5)] public List<string> branch9;
    [TextArea(2, 5)] public List<string> branch10;

    private Coroutine activeDialogue;

    public void StartDialogueBranch(int branch)
    {
        if (activeDialogue != null)
            StopCoroutine(activeDialogue);

        List<string> selectedBranch = GetBranch(branch);

        if (selectedBranch != null && selectedBranch.Count > 0)
        {
            activeDialogue = StartCoroutine(ShowDialogueSequence(selectedBranch));
        }
        else
        {
            Debug.LogWarning($"Branch {branch} is null or empty!");
        }
    }

    private List<string> GetBranch(int branchNumber)
    {
        switch (branchNumber)
        {
            case 0: return branch0;
            case 1: return branch1;
            case 2: return branch2;
            case 3: return branch3;
            case 4: return branch4;
            case 5: return branch5;
            case 6: return branch6;
            case 7: return branch7;
            case 8: return branch8;
            case 9: return branch9;
            case 10: return branch10;
            default: return null;
        }
    }

    private IEnumerator ShowDialogueSequence(List<string> dialogues)
    {
        if (dialogueTextUI == null)
        {
            Debug.LogWarning("DialogueTextUI is null!");
            yield break;
        }

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        dialogueTextUI.enabled = true;

        foreach (string line in dialogues)
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
    }
}
