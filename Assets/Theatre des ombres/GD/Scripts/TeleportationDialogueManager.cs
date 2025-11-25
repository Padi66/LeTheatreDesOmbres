using UnityEngine;
using System.Collections;

public class TeleportationDialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueSequence dialogueSequence;
    [SerializeField] private PiedestalUP piedestalController;
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private GameObject wallTrigger1;
    [SerializeField] private GameObject wallTrigger2;

    [Header("State Tracking")]
    private bool introPlayed = false;
    private bool puzzleCompleted = false;

    private void OnEnable()
    {
        StoryManager.OnCubePlaced += OnCubePlacedHandler;
    }

    private void OnDisable()
    {
        StoryManager.OnCubePlaced -= OnCubePlacedHandler;
    }

    private void Start()
    {
        if (wallTrigger1 != null) wallTrigger1.SetActive(false);
        if (wallTrigger2 != null) wallTrigger2.SetActive(false);

        StartCoroutine(PlayIntroAndRaisePedestals());
    }

    private IEnumerator PlayIntroAndRaisePedestals()
    {
        yield return new WaitForSeconds(1f);

        if (piedestalController != null)
        {
            piedestalController.UpGreen();
            piedestalController.UpOrange();
            piedestalController.UpPurple();
            Debug.Log("All pedestals are rising!");
        }

        yield return new WaitForSeconds(0.5f);

        if (dialogueSequence != null)
        {
            dialogueSequence.StartDialogueBranch(0);
            introPlayed = true;
            Debug.Log("Intro dialogue started. Waiting for cube placements...");
        }
        else
        {
            Debug.LogError("DialogueSequence is not assigned!");
        }
    }

    private void OnCubePlacedHandler(string socketName, string cubeName)
    {
        if (!introPlayed)
        {
            Debug.Log("Intro not played yet, ignoring cube placement");
            return;
        }

        if (string.IsNullOrEmpty(cubeName))
        {
            Debug.Log($"Cube removed from {socketName}");
            return;
        }

        Debug.Log($"Cube placed: {cubeName} in {socketName}");

        PlayDialogueForCubePlacement(socketName, cubeName);

        StartCoroutine(CheckAfterDelay());
    }

    private IEnumerator CheckAfterDelay()
    {
        yield return new WaitForEndOfFrame();
        CheckIfAllSocketsFilled();
    }

    private void PlayDialogueForCubePlacement(string socketName, string cubeName)
    {
        if (socketName == "Green")
        {
            if (cubeName == "CubeGreen")
            {
                dialogueSequence.StartDialogueBranch(1);
            }
            else if (cubeName == "CubeOrange")
            {
                dialogueSequence.StartDialogueBranch(2);
            }
            else if (cubeName == "CubePurple")
            {
                dialogueSequence.StartDialogueBranch(3);
            }
        }
        else if (socketName == "Orange")
        {
            if (cubeName == "CubeGreen")
            {
                dialogueSequence.StartDialogueBranch(4);
            }
            else if (cubeName == "CubeOrange")
            {
                dialogueSequence.StartDialogueBranch(5);
            }
            else if (cubeName == "CubePurple")
            {
                dialogueSequence.StartDialogueBranch(6);
            }
        }
        else if (socketName == "Purple")
        {
            if (cubeName == "CubeGreen")
            {
                dialogueSequence.StartDialogueBranch(7);
            }
            else if (cubeName == "CubeOrange")
            {
                dialogueSequence.StartDialogueBranch(8);
            }
            else if (cubeName == "CubePurple")
            {
                dialogueSequence.StartDialogueBranch(9);
            }
        }
    }

    private void CheckIfAllSocketsFilled()
    {
        if (puzzleCompleted) return;

        if (storyManager == null)
        {
            Debug.LogError("StoryManager is not assigned!");
            return;
        }

        bool greenFilled = !string.IsNullOrEmpty(storyManager._cubeInGreen);
        bool orangeFilled = !string.IsNullOrEmpty(storyManager._cubeInOrange);
        bool purpleFilled = !string.IsNullOrEmpty(storyManager._cubeInPurple);

        Debug.Log($"Socket Status - Green: {greenFilled} ({storyManager._cubeInGreen}), Orange: {orangeFilled} ({storyManager._cubeInOrange}), Purple: {purpleFilled} ({storyManager._cubeInPurple})");

        if (greenFilled && orangeFilled && purpleFilled)
        {
            Debug.Log("All 3 sockets are filled! Completing puzzle...");
            puzzleCompleted = true;

            dialogueSequence.StartDialogueBranch(10);

            StartCoroutine(WaitAndCompletePuzzle());
        }
    }

    private IEnumerator WaitAndCompletePuzzle()
    {
        yield return new WaitForSeconds(3f);

        CompletePuzzle();
    }

    private void CompletePuzzle()
    {
        Debug.Log("Puzzle complete! Activating wall triggers...");

        if (wallTrigger1 != null)
        {
            wallTrigger1.SetActive(true);
            Debug.Log("WallTrigger1 activated!");
        }
        else
        {
            Debug.LogWarning("WallTrigger1 is not assigned!");
        }

        if (wallTrigger2 != null)
        {
            wallTrigger2.SetActive(true);
            Debug.Log("WallTrigger2 activated!");
        }
        else
        {
            Debug.LogWarning("WallTrigger2 is not assigned!");
        }
    }
}
