using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Header("Scene Settings")]
    public int sceneToLoadBuildIndex;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    [Header("Dialogue Settings")]
    public DialogueSequence dialogueSequence;
    public float delayBeforeDialogue = 2f;

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            StartCoroutine(TeleportSequence());
        }
    }

    private IEnumerator TeleportSequence()
    {
        yield return StartCoroutine(FadeToBlack());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadBuildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (dialogueSequence == null)
        {
            dialogueSequence = FindFirstObjectByType<DialogueSequence>();
        }

        yield return StartCoroutine(FadeFromBlack());

        yield return new WaitForSeconds(delayBeforeDialogue);

        if (dialogueSequence != null)
        {
            dialogueSequence.StartDialogueBranch(1);
        }

        isTeleporting = false;
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
    }
}
