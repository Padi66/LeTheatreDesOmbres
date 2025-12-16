using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using Unity.XR.CoreUtils;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    
    public static SceneTransitionManager Instance => instance;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    [Header("Dialogue Settings")]
    public float delayBeforeDialogue = 2f;

    [Header("Camera Settings")]
    public bool resetCameraRotationOnSceneLoad = true;
    public float targetCameraYRotation = 0f;

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
            Debug.LogWarning($"Duplicate SceneTransitionManager found in {gameObject.scene.name}, destroying...");
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
                Debug.Log($"✅ FadeCanvas camera assigned: {cam.name}");
            }
            else
            {
                Debug.LogWarning("⚠️ No camera found for FadeCanvas!");
            }
        }
    }

    private Camera FindSceneCamera()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            Debug.Log($"Found Camera.main: {cam.name}");
            return cam;
        }

        XROrigin xrOrigin = FindFirstObjectByType<XROrigin>();
        if (xrOrigin != null && xrOrigin.Camera != null)
        {
            Debug.Log($"Found camera from XROrigin: {xrOrigin.Camera.name}");
            return xrOrigin.Camera;
        }

        Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        Debug.Log($"Searching through {allCameras.Length} cameras in scene");

        foreach (Camera c in allCameras)
        {
            if (c.enabled && c.gameObject.activeInHierarchy)
            {
                Debug.Log($"Found active camera: {c.name}");
                return c;
            }
        }

        if (allCameras.Length > 0)
        {
            Debug.LogWarning($"No active camera found, using first camera: {allCameras[0].name}");
            return allCameras[0];
        }

        Debug.LogError("NO CAMERA FOUND IN SCENE!");
        return null;
    }

    public static void TeleportToScene(int sceneIndex, int dialogueBranch, ContinuousMoveProvider move, ContinuousTurnProvider turn, bool disableMovement = false)
    {
        if (instance != null)
        {
            instance.moveProvider = move;
            instance.turnProvider = turn;
            instance.currentDialogueBranch = dialogueBranch;
            instance.StartCoroutine(instance.TransitionToScene(sceneIndex, disableMovement));
        }
        else
        {
            Debug.LogError("SceneTransitionManager instance is null!");
        }
    }

    public IEnumerator TransitionToScene(int sceneIndex, bool disableMovement = false)
    {
        Debug.Log($"Starting transition to scene {sceneIndex} (disableMovement: {disableMovement})");

        sceneFadeComplete = false;

        if (disableMovement)
        {
            DisableMovementInCurrentScene();
            DisableControllerRays();
        }

        yield return StartCoroutine(FadeToBlack());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log($"Scene {sceneIndex} loaded - configuring NEW scene");

        yield return new WaitForSecondsRealtime(0.1f);

        SetupFadeCanvas();

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            fadeCanvasGroup.blocksRaycasts = true;
            Debug.Log("FadeCanvas set to black (alpha = 1) in new scene");
        }

        if (resetCameraRotationOnSceneLoad)
        {
            ResetCameraRotation();
        }

        if (disableMovement)
        {
            DisableControllerRays();
            DisableMovementInNewScene();
        }

        yield return new WaitForSecondsRealtime(0.2f);

        SceneFadeScreen sceneFadeScreen = FindFirstObjectByType<SceneFadeScreen>();

        if (sceneFadeScreen != null)
        {
            Debug.Log("Found SceneFadeScreen, starting fade out");
            sceneFadeScreen.FadeOut(fadeDuration, () => OnSceneFadeComplete(disableMovement));
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
        
        if (disableMovement)
        {
            EnableControllerRays();
        }

        if (sceneFadeScreen == null)
        {
            OnSceneFadeComplete(disableMovement);
        }
    }

    private void ResetCameraRotation()
    {
        XROrigin xrOrigin = FindFirstObjectByType<XROrigin>();

        if (xrOrigin != null)
        {
            Camera mainCamera = xrOrigin.Camera;

            if (mainCamera != null)
            {
                float currentCameraYRotation = mainCamera.transform.eulerAngles.y;
                float rotationDifference = targetCameraYRotation - currentCameraYRotation;

                Vector3 currentOriginRotation = xrOrigin.transform.eulerAngles;
                xrOrigin.transform.eulerAngles = new Vector3(
                    currentOriginRotation.x,
                    currentOriginRotation.y + rotationDifference,
                    currentOriginRotation.z
                );

                Debug.Log($"Camera rotation reset - Target: {targetCameraYRotation}°, Camera was: {currentCameraYRotation}°, XROrigin rotated by: {rotationDifference}°");
            }
            else
            {
                Debug.LogWarning("XROrigin found but Camera is null!");
            }
        }
        else
        {
            Debug.LogWarning("No XROrigin found in the new scene!");
        }
    }

    private void OnSceneFadeComplete(bool disableMovement)
    {
        Debug.Log("SceneFadeScreen complete");
        sceneFadeComplete = true;

        if (!disableMovement)
        {
            EnableMovementInNewScene();
        }

        StartCoroutine(DelayedDialogue(disableMovement));
    }

    private IEnumerator DelayedDialogue(bool disableMovement)
    {
        yield return new WaitForSecondsRealtime(delayBeforeDialogue);
        TriggerDialogue(disableMovement);
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
            Debug.LogError("❌ fadeCanvasGroup is null!");
            yield break;
        }

        Debug.Log("Starting FadeToBlack...");
        fadeCanvasGroup.blocksRaycasts = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
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
            Debug.LogError("❌ fadeCanvasGroup is null!");
            yield break;
        }

        Debug.Log("Starting FadeFromBlack...");
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
        Debug.Log("FadeFromBlack complete");
    }

    private void DisableMovementInCurrentScene()
    {
        ContinuousMoveProvider[] moveProviders = FindObjectsByType<ContinuousMoveProvider>(FindObjectsSortMode.None);
        ContinuousTurnProvider[] continuousTurnProviders = FindObjectsByType<ContinuousTurnProvider>(FindObjectsSortMode.None);
        SnapTurnProvider[] snapTurnProviders = FindObjectsByType<SnapTurnProvider>(FindObjectsSortMode.None);

        foreach (var move in moveProviders)
        {
            move.enabled = false;
            Debug.Log($"Disabled ContinuousMoveProvider on {move.gameObject.name} in CURRENT scene");
        }

        foreach (var turn in continuousTurnProviders)
        {
            turn.enabled = false;
            Debug.Log($"Disabled ContinuousTurnProvider on {turn.gameObject.name} in CURRENT scene");
        }

        foreach (var snapTurn in snapTurnProviders)
        {
            snapTurn.enabled = false;
            Debug.Log($"Disabled SnapTurnProvider on {snapTurn.gameObject.name} in CURRENT scene");
        }
    }

    private void DisableMovementInNewScene()
    {
        ContinuousMoveProvider[] moveProviders = FindObjectsByType<ContinuousMoveProvider>(FindObjectsSortMode.None);
        ContinuousTurnProvider[] continuousTurnProviders = FindObjectsByType<ContinuousTurnProvider>(FindObjectsSortMode.None);
        SnapTurnProvider[] snapTurnProviders = FindObjectsByType<SnapTurnProvider>(FindObjectsSortMode.None);

        foreach (var move in moveProviders)
        {
            move.enabled = false;
            Debug.Log($"Disabled ContinuousMoveProvider on {move.gameObject.name} in NEW scene");
        }

        foreach (var turn in continuousTurnProviders)
        {
            turn.enabled = false;
            Debug.Log($"Disabled ContinuousTurnProvider on {turn.gameObject.name} in NEW scene");
        }

        foreach (var snapTurn in snapTurnProviders)
        {
            snapTurn.enabled = false;
            Debug.Log($"Disabled SnapTurnProvider on {snapTurn.gameObject.name} in NEW scene");
        }
    }

    private void EnableMovementInNewScene()
    {
        ContinuousMoveProvider[] moveProviders = FindObjectsByType<ContinuousMoveProvider>(FindObjectsSortMode.None);
        ContinuousTurnProvider[] continuousTurnProviders = FindObjectsByType<ContinuousTurnProvider>(FindObjectsSortMode.None);
        SnapTurnProvider[] snapTurnProviders = FindObjectsByType<SnapTurnProvider>(FindObjectsSortMode.None);

        foreach (var move in moveProviders)
        {
            move.enabled = true;
            Debug.Log($"Enabled ContinuousMoveProvider on {move.gameObject.name}");
        }

        foreach (var turn in continuousTurnProviders)
        {
            turn.enabled = true;
            Debug.Log($"Enabled ContinuousTurnProvider on {turn.gameObject.name}");
        }

        foreach (var snapTurn in snapTurnProviders)
        {
            snapTurn.enabled = true;
            Debug.Log($"Enabled SnapTurnProvider on {snapTurn.gameObject.name}");
        }
    }

    public void EnableMovementAfterDialogue()
    {
        EnableMovementInNewScene();
        Debug.Log("Movement enabled after dialogue");
    }

    private void TriggerDialogue(bool disableMovement)
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
            Debug.Log("No DialogueSequence found - enabling movement immediately");
        
            if (disableMovement)
            {
                EnableMovementInNewScene();
            }
        }
    }
}
