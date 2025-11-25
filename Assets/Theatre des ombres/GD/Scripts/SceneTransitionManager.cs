using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    [Header("Dialogue Settings")]
    public float delayBeforeDialogue = 2f;

    private ContinuousMoveProvider moveProvider;
    private ContinuousTurnProvider turnProvider;
    private int currentDialogueBranch = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void TeleportToScene(int sceneIndex, int dialogueBranch, ContinuousMoveProvider move, ContinuousTurnProvider turn)
    {
        if (instance != null)
        {
            instance.moveProvider = move;
            instance.turnProvider = turn;
            instance.currentDialogueBranch = dialogueBranch;
            instance.StartCoroutine(instance.TransitionToScene(sceneIndex));
        }
    }

   
    public IEnumerator TransitionToScene(int sceneIndex)
    {
        DisableMovement();

        yield return StartCoroutine(FadeToBlack());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(FadeFromBlack());

        yield return new WaitForSeconds(0.1f);

        FindAndEnableMovementInNewScene();

        yield return new WaitForSeconds(delayBeforeDialogue);

        TriggerDialogue();
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            }
            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
        }
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            }
            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
    }

    private void DisableMovement()
    {
        if (moveProvider != null)
        {
            moveProvider.enabled = false;
        }

        if (turnProvider != null)
        {
            turnProvider.enabled = false;
        }

        Debug.Log("Movement disabled for teleportation");
    }

    private void FindAndEnableMovementInNewScene()
    {
        ContinuousMoveProvider newMove = FindFirstObjectByType<ContinuousMoveProvider>();
        ContinuousTurnProvider newTurn = FindFirstObjectByType<ContinuousTurnProvider>();

        if (newMove != null)
        {
            newMove.enabled = true;
            Debug.Log("Movement enabled in new scene");
        }

        if (newTurn != null)
        {
            newTurn.enabled = true;
            Debug.Log("Turn enabled in new scene");
        }
    }

    private void TriggerDialogue()
    {
        DialogueSequence dialogueSequence = FindFirstObjectByType<DialogueSequence>();

        if (dialogueSequence != null)
        {
            if (!dialogueSequence.gameObject.activeInHierarchy)
            {
                dialogueSequence.gameObject.SetActive(true);
            }

            dialogueSequence.StartDialogueBranch(currentDialogueBranch);
            Debug.Log($"Dialogue branch {currentDialogueBranch} triggered");
        }
        else
        {
            Debug.LogWarning("DialogueSequence not found in the new scene");
        }
    }
}
