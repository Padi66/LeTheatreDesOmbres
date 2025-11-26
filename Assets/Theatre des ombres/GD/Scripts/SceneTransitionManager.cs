using System.Collections;
using System.Collections.Generic;
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

    private Canvas fadeCanvas;
    private ContinuousMoveProvider moveProvider;
    private ContinuousTurnProvider turnProvider;
    private int currentDialogueBranch = 1;
    private bool sceneFadeComplete = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fadeCanvas = GetComponentInChildren<Canvas>();

            if (fadeCanvas != null)
            {
                DontDestroyOnLoad(fadeCanvas.gameObject);
                Debug.Log($"FadeCanvas marked as DontDestroyOnLoad: {fadeCanvas.name}");
            }
            else
            {
                Debug.LogError("No Canvas found as child of SceneTransitionManager!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupFadeCanvas();
    }

    private Canvas GetFadeCanvas()
    {
        if (fadeCanvas == null)
        {
            fadeCanvas = GetComponentInChildren<Canvas>();
        }
        return fadeCanvas;
    }

    private void SetupFadeCanvas()
    {
        Canvas canvas = GetFadeCanvas();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.sortingOrder = 100;
            canvas.planeDistance = 0.1f;

            Camera cam = FindSceneCamera();
            if (cam != null)
            {
                canvas.worldCamera = cam;
            }
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    private Camera FindSceneCamera()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            return cam;
        }

        Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);

        foreach (Camera c in allCameras)
        {
            if (c.enabled && c.gameObject.activeInHierarchy)
            {
                return c;
            }
        }

        if (allCameras.Length > 0)
        {
            return allCameras[0];
        }

        return null;
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
        else
        {
            Debug.LogError("SceneTransitionManager instance is null!");
        }
    }

    public IEnumerator TransitionToScene(int sceneIndex)
    {
        Debug.Log($"Starting transition to scene {sceneIndex}");

        sceneFadeComplete = false;

        DisableMovement();
        DisableControllerRays();

        yield return StartCoroutine(FadeToBlack());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log($"Scene {sceneIndex} loaded - disabling NEW scene LineVisuals");

        DisableControllerRays();

        yield return new WaitForSeconds(0.2f);

        SceneFadeScreen sceneFadeScreen = FindFirstObjectByType<SceneFadeScreen>();

        if (sceneFadeScreen != null)
        {
            Debug.Log("Found SceneFadeScreen, starting fade out");
            sceneFadeScreen.FadeOut(fadeDuration, OnSceneFadeComplete);
        }
        else
        {
            Debug.LogWarning("No SceneFadeScreen found in new scene");
            sceneFadeComplete = true;
        }

        yield return StartCoroutine(FadeFromBlack());

        Debug.Log("FadeFromBlack done, waiting for SceneFadeScreen to complete");

        while (!sceneFadeComplete)
        {
            yield return null;
        }

        Debug.Log("Both fades complete, enabling controller rays NOW");
        EnableControllerRays();

        if (sceneFadeScreen == null)
        {
            OnSceneFadeComplete();
        }
    }

    private void OnSceneFadeComplete()
    {
        Debug.Log("SceneFadeScreen complete");
        sceneFadeComplete = true;
        FindAndEnableMovementInNewScene();
        StartCoroutine(DelayedDialogue());
    }

    private IEnumerator DelayedDialogue()
    {
        yield return new WaitForSeconds(delayBeforeDialogue);
        TriggerDialogue();
    }

    private void DisableControllerRays()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int disabledCount = 0;
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "LineVisual" && obj.activeSelf)
            {
                obj.SetActive(false);
                disabledCount++;
                Debug.Log($"Disabled LineVisual: {GetGameObjectPath(obj)}");
            }
        }

        Debug.Log($"Disabled {disabledCount} line visuals");
    }

    private void EnableControllerRays()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        int enabledCount = 0;
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "LineVisual" && !obj.activeSelf)
            {
                obj.SetActive(true);
                enabledCount++;
                Debug.Log($"Enabled LineVisual: {GetGameObjectPath(obj)}");
            }
        }

        Debug.Log($"Re-enabled {enabledCount} line visuals");
    }

    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return "/" + path;
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        fadeCanvasGroup.blocksRaycasts = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        Debug.Log("FadeToBlack complete");
    }

    private IEnumerator FadeFromBlack()
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
        Debug.Log("FadeFromBlack complete");
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
    }

    private void FindAndEnableMovementInNewScene()
    {
        ContinuousMoveProvider newMove = FindFirstObjectByType<ContinuousMoveProvider>();
        ContinuousTurnProvider newTurn = FindFirstObjectByType<ContinuousTurnProvider>();

        if (newMove != null)
        {
            newMove.enabled = true;
        }

        if (newTurn != null)
        {
            newTurn.enabled = true;
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
        }
    }
}
