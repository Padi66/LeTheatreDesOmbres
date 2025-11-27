using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ActivateStory : MonoBehaviour
{
    [SerializeField] private XRPushButton _button;
    [SerializeField] private Transform _attachPositionStart;
    [SerializeField] private Transform _attachPositionEnd;
    [SerializeField] private Transform _socketAttach;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _delayAfterAnimation = 0.5f;

    [SerializeField] private SocketMenu _socketMenuRef;
    [SerializeField] private LevelManager _levelManager;
    

    private bool _hasBeenPressed = false;

    void OnEnable()
    {
        _button.onPress.AddListener(OnButtonPressed);
    }

    void OnDisable()
    {
        _button.onPress.RemoveListener(OnButtonPressed);
    }

    void OnButtonPressed()
    {
        if (_hasBeenPressed)
        {
            Debug.LogWarning("Bouton déjà pressé, action en cours - ignorer");
            return;
        }

        if (_socketMenuRef._isInSocket)
        {
            Debug.Log("✓ Bouton pressé - cube dans socket détecté!");

            string cubeType = GetCubeTypeInSocket();
            Debug.Log($"Type de cube détecté: '{cubeType}'");

            if (string.IsNullOrEmpty(cubeType))
            {
                Debug.LogError("Impossible de déterminer le type de cube!");
                return;
            }

            _hasBeenPressed = true;

            GameObject cubeObject = GetCubeGameObject();
            if (cubeObject != null)
            {
                StartCoroutine(AnimateCubeAndTrigger(cubeObject, cubeType));
            }
            else
            {
                Debug.LogError("Impossible de récupérer le GameObject du cube!");
            }
        }
        else
        {
            Debug.LogWarning("✗ Bouton pressé mais aucun cube dans la socket!");
        }
    }

    private GameObject GetCubeGameObject()
    {
        var socketInteractor = _socketMenuRef.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        if (socketInteractor != null && socketInteractor.hasSelection)
        {
            var interactable = socketInteractor.interactablesSelected[0];
            return interactable.transform.gameObject;
        }

        return null;
    }

    private string GetCubeTypeInSocket()
    {
        var socketInteractor = _socketMenuRef.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        if (socketInteractor != null && socketInteractor.hasSelection)
        {
            var interactable = socketInteractor.interactablesSelected[0];
            GameObject cube = interactable.transform.gameObject;

            if (cube.GetComponent<CubeGreen>())
            {
                return "CubeGreen";
            }
            else if (cube.GetComponent<CubeOrange>())
            {
                return "CubeOrange";
            }
            else if (cube.GetComponent<CubePurple>())
            {
                return "CubePurple";
            }
        }

        return null;
    }

    IEnumerator AnimateCubeAndTrigger(GameObject cube, string cubeType)
    {
        Debug.Log("Début animation du cube");

        LockCubeGrab(cube);

        Vector3 startPos = _attachPositionStart.position;
        Vector3 endPos = _attachPositionEnd.position;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            cube.transform.position = Vector3.Lerp(startPos, endPos, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cube.transform.position = endPos;

        Debug.Log("Animation terminée - verrouillage final");
        LockCubeFinal(cube);

        Debug.Log("Attente avant event");
        yield return new WaitForSeconds(_delayAfterAnimation);

        Debug.Log($"Déclenchement de l'action finale - Type de cube: '{cubeType}'");

        ExecuteMenuActionWithCubeType(cubeType);
        
        
    }

    private void LockCubeGrab(GameObject cube)
    {
        var grabInteractable = cube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            Debug.Log($"Cube {cube.name} - Grab désactivé");
        }

        var socketInteractor = _socketMenuRef.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        if (socketInteractor != null)
        {
            socketInteractor.enabled = false;
            Debug.Log("Socket désactivée");
        }

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            Debug.Log("Rigidbody mis en kinematic pour l'animation");
        }
    }

    private void LockCubeFinal(GameObject cube)
    {
        Debug.Log($"Cube {cube.name} verrouillé définitivement");
    }
    
    public void ExecuteMenuActionWithCubeType(string cubeType)
    {
        Debug.Log($"=== ExecuteMenuActionWithCubeType appelé - Type: '{cubeType}' ===");

        if (string.IsNullOrEmpty(cubeType))
        {
            Debug.LogWarning("Type de cube vide!");
            return;
        }

        if (cubeType == "CubeGreen")
        {
            Debug.Log("✓ Cube VERT - Chargement Level 1");
            _levelManager.LoadLevel1();
        }
        else if (cubeType == "CubePurple")
        {
            Debug.Log("✓ Cube VIOLET - Fermeture du jeu");
            _levelManager.Quit();
        }
        else
        {
            Debug.LogWarning($"Type de cube non géré: '{cubeType}'");
        }
    }
}
