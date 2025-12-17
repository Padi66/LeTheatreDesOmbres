using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Content.Interaction;

public class ActivateMenu : MonoBehaviour
{
    [SerializeField] private XRPushButton _button;
    [SerializeField] private Transform _attachPositionStart;
    [SerializeField] private Transform _attachPositionEnd;
    [SerializeField] private Transform _socketAttach;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _delayAfterAnimation = 0.5f;
    [SerializeField] private Transform _animationPrefab;

    [SerializeField] private SocketMenu _socketMenuRef;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameObject _buttonVisual;
    [SerializeField] private Animator _handAnimator;
    private const string IS_TICKET_IN_SOCKET = "IsTicketInSocket";
    private float _durationAnim= 0.5f;
    
    private int _outlineLayer;
    private int _defaultLayer;

    
    public Transform _startPosition;
    public Transform _endPosition;
    public Transform _rideau;
    
    
    private AsyncOperation _preloadedScene;
    private int pendingSceneIndex = -1;
    private bool waitingForDialogues = false;
    public bool _isLaunched = false;
    [SerializeField] private SceneTransitionManager _transition;
    [SerializeField] private AudioSource _sound;
    [SerializeField] private AudioSource _roboticVoice;
    
    
    void Awake()
    {
        _outlineLayer = LayerMask.NameToLayer("Outline");
        _defaultLayer = LayerMask.NameToLayer("Default");
    
        if (_buttonVisual == null)
        {
            _buttonVisual = gameObject;
        }
    }


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
        _sound.Play();
        if (_hasBeenPressed)
        {
            Debug.LogWarning("Bouton déjà pressé, action en cours - ignorer");
            
            return;
        }

        if (_socketMenuRef._isInSocket)
        {
            Debug.Log("Bouton pressé - cube dans socket détecté");

            string cubeType = GetCubeTypeInSocket();
            Debug.Log($"Type de cube détecté: '{cubeType}'");
            

            if (string.IsNullOrEmpty(cubeType))
            {
                Debug.LogError("Impossible de déterminer le type de cube");
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
                Debug.LogError("Impossible de récupérer le GameObject du cube");
            }
            
        }
        else
        {
            Debug.LogWarning("Bouton pressé mais aucun cube dans la socket");
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

            if (cube.GetComponent<PlayTicket>())
            {
                return "PlayTicket";
                
            }
            else if (cube.GetComponent<QuitTicket>())
            {
                return "QuitTicket";
            }
        }

        return null;
    }

    IEnumerator AnimateCubeAndTrigger(GameObject cube, string cubeType)
    {
        Debug.Log("Début animation du cube");

        LockCubeGrab(cube);
    
        cube.transform.SetParent(_socketAttach);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
        /*Vector3 startPos = _attachPositionStart.position;
        Vector3 endPos = _attachPositionEnd.position;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _socketAttach.position = Vector3.Lerp(startPos, endPos, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _socketAttach.position = endPos;*/
        
        if (_handAnimator != null)
        {
            _handAnimator.SetTrigger(IS_TICKET_IN_SOCKET);
        }
        
        StartCoroutine(CloseCurtains());

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
            Debug.LogWarning("Type de cube vide");
            return;
        }

        if (cubeType == "PlayTicket")
        {
            Debug.Log("Chargement Level 1");
            _roboticVoice.Play();
            StartCoroutine(TransitionAfterDelay(1));
        }
        else if (cubeType == "QuitTicket")
        {
            Debug.Log("Fermeture du jeu");
            _levelManager.Quit();
        }
        else
        {
            Debug.LogWarning($"Type de cube non géré: '{cubeType}'");
        }
    }
    
    void Update()
    {
        CheckSocketState();
    }

    private void CheckSocketState()
    {
        if (_socketMenuRef._isInSocket && !_hasBeenPressed)
        {
            SetLayerRecursively(_buttonVisual, _outlineLayer);
        }
        else
        {
            SetLayerRecursively(_buttonVisual, _defaultLayer);
        }
    }
    
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;

        if (obj.GetComponent<Collider>() == null)
        {
            obj.layer = layer;
        }

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
    
    IEnumerator CloseCurtains()
    {
        yield return new WaitForSeconds(4.5f);
        float elapsed = 0f;

        while (elapsed < _durationAnim)
        {
            _rideau.position = Vector3.Lerp(_startPosition.position, _endPosition.position, elapsed / _durationAnim);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rideau.position = _endPosition.position;
    }
    
    private IEnumerator TransitionAfterDelay(int sceneIndex)
    {
        yield return new WaitForSeconds(0f);

        Debug.Log($"[ActivateMenu] Starting transition to scene {sceneIndex}");
    
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.StartCoroutine(
                SceneTransitionManager.Instance.TransitionToScene(sceneIndex, disableMovement: false)
            );
        }
        else
        {
            Debug.LogError("[ActivateMenu] SceneTransitionManager.Instance is null!");
        }
    }


    }
    




