using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSequence : MonoBehaviour
{
    [Header("UI Reference")] public TextMeshProUGUI dialogueTextUI;

    [Header("Dialogue Settings")] public float typewriterDelay = 0.05f;
    public float displayTime = 2f;

    [Header("Audio Preloading")] [Tooltip("AudioSources � pr�charger au Start pour �viter les freezes")]
    public List<AudioSource> audioSourcesToPreload = new List<AudioSource>();

    [Header("Events")] public UnityEvent onAllDialoguesComplete;

    [Header("Dialogue Branches")] [TextArea(2, 5)]
    public List<string> branch0;

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

    [Header("Persistent Branches")] [Tooltip("Branches dont le dernier élément reste affiché")]
    public List<int> persistentBranches = new List<int> { 1, 5, 8 };

    private int _currentBranch = -1;
    private string _lastDialogueText = "";
    private Coroutine activeDialogue;
    private Queue<int> branchQueue = new Queue<int>();
    private bool isPlaying = false;

    private void Start()
    {
        PreloadAudioClips();
    }

    private void PreloadAudioClips()
    {
        if (audioSourcesToPreload.Count > 0)
        {
            foreach (AudioSource audioSource in audioSourcesToPreload)
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.clip.LoadAudioData();
                }
            }
        }
    }

    public void StartDialogueBranch(int branch)

    {
        if (_currentBranch == branch && isPlaying)
        {
            Debug.Log($"Branch {branch} is already playing, ignoring.");
            return;
        }

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
            case 23: return branch23;
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
        _currentBranch = branchNumber;

        Debug.Log($"Playing Branch {branchNumber} with {dialogues.Count} lines");

        for (int i = 0; i < dialogues.Count; i++)
        {
            string line = dialogues[i];
            bool isLastLine = (i == dialogues.Count - 1);
            dialogueTextUI.text = "";

            float startTime = Time.realtimeSinceStartup;
            int characterIndex = 0;

            while (characterIndex < line.Length)
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                int targetCharCount = Mathf.FloorToInt(elapsedTime / typewriterDelay);

                while (characterIndex < targetCharCount && characterIndex < line.Length)
                {
                    dialogueTextUI.text += line[characterIndex];
                    characterIndex++;
                }

                yield return null;
            }

            if (isLastLine && persistentBranches.Contains(branchNumber))
            {
                _lastDialogueText = line;
            }

            float displayStartTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - displayStartTime < displayTime)
            {
                yield return null;
            }
        }

        if (persistentBranches.Contains(branchNumber))
        {
            dialogueTextUI.text = _lastDialogueText;
            Debug.Log($"Branch {branchNumber} finished - Last line kept displayed");
        }
        else
        {
            dialogueTextUI.text = "";
            Debug.Log($"Branch {branchNumber} finished - Text cleared");
        }

        isPlaying = false;
        activeDialogue = null;
        _currentBranch = -1;

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
}
