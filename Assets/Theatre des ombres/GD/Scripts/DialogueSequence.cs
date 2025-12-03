using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSequence : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI dialogueTextUI;

    [Header("Dialogue Settings")]
    public float typewriterDelay = 0.05f;
    public float displayTime = 2f;

    [Header("Events")]
    public UnityEvent onAllDialoguesComplete;

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
    [TextArea(2, 5)] public List<string> branch11;
    [TextArea(2, 5)] public List<string> branch12;
    [TextArea(2, 5)] public List<string> branch13;
    [TextArea(2, 5)] public List<string> branch14;
    [TextArea(2, 5)] public List<string> branch15;
    [TextArea(2, 5)] public List<string> branch16;
    [TextArea(2, 5)] public List<string> branch17;
    [TextArea(2, 5)] public List<string> branch18;
    [TextArea(2, 5)] public List<string> branch19;
    [TextArea(2, 5)] public List<string> branch20;
    [TextArea(2, 5)] public List<string> branch21;
    [TextArea(2, 5)] public List<string> branch22;
    [TextArea(2, 5)] public List<string> branch23;

    private Coroutine activeDialogue;
    private Queue<int> branchQueue = new Queue<int>();
    private bool isPlaying = false;

    public void StartDialogueBranch(int branch)
    {
        if (isPlaying)
        {
            if (!branchQueue.Contains(branch))
            {
                branchQueue.Enqueue(branch);
                Debug.Log($"Branch {branch} queued. Queue size: {branchQueue.Count}");
            }
            else
            {
                Debug.Log($"Branch {branch} already in queue, skipping.");
            }
        }
        else
        {
            PlayBranch(branch);
        }
    }

    private void PlayBranch(int branch)
    {
        List<string> selectedBranch = GetBranch(branch);

        if (selectedBranch != null && selectedBranch.Count > 0)
        {
            if (activeDialogue != null)
                StopCoroutine(activeDialogue);

            activeDialogue = StartCoroutine(ShowDialogueSequence(selectedBranch, branch));
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
            case 11: return branch11;
            case 12: return branch12;
            case 13: return branch13;
            case 14: return branch14;
            case 15: return branch15;
            case 16: return branch16;
            case 17: return branch17;
            case 18: return branch18;
            case 19: return branch19;
            case 20: return branch20;
            case 21: return branch21;
            case 22: return branch22;
            default: return null;
        }
    }

    private IEnumerator ShowDialogueSequence(List<string> dialogues, int branchNumber)
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
        isPlaying = true;

        Debug.Log($"Playing Branch {branchNumber} with {dialogues.Count} lines");

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
        isPlaying = false;
        activeDialogue = null;

        Debug.Log($"Branch {branchNumber} finished");

        if (branchQueue.Count > 0)
        {
            int nextBranch = branchQueue.Dequeue();
            Debug.Log($"Playing next queued branch: {nextBranch}. Remaining in queue: {branchQueue.Count}");
            PlayBranch(nextBranch);
        }
        else
        {
            Debug.Log("All dialogues complete! Invoking onAllDialoguesComplete event.");
            onAllDialoguesComplete?.Invoke();
        }
    }

    public bool IsPlaying => isPlaying;
    public bool HasQueuedDialogues => branchQueue.Count > 0;
}
