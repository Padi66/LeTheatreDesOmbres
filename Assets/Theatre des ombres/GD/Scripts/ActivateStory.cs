using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ActivateStory : MonoBehaviour
{
    [SerializeField] private XRPushButton _button;
    [SerializeField] private GameObject _socketMenu;
    [SerializeField] private Transform _positionEnd;
    [SerializeField] private float _duration = 3f;
    
    [SerializeField] private SocketPurple _socketPurpleRef;

    private string _lockedCubeName;
    private Vector3 _startPosition;

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
        Debug.Log($"Bouton pressé ! Cube dans socket : {_socketPurpleRef._isInSocket}");
        
        if (_socketPurpleRef._isInSocket)
        {
            XRSocketInteractor socketInteractor = _socketPurpleRef.GetComponent<XRSocketInteractor>();
            
            if (socketInteractor != null && socketInteractor.hasSelection)
            {
                _lockedCubeName = socketInteractor.interactablesSelected[0].transform.gameObject.name;
                Debug.Log($"Cube verrouillé : {_lockedCubeName}");
                
                StoryManager.OnSocketStateChanged?.Invoke("Purple", true);
                StoryManager.OnCubePlaced?.Invoke("Purple", _lockedCubeName);
            }
            
            LockCubeInSocket(socketInteractor);
            StoryManager.OnPushButton?.Invoke();
            StartCoroutine(MoveTicket());
        }
        else
        {
            Debug.Log("Pas de cube dans le socket !");
        }
    }
    
    private void LockCubeInSocket(XRSocketInteractor socket)
    {
        if (socket != null && socket.hasSelection)
        {
            IXRSelectInteractable interactable = socket.interactablesSelected[0];
            GameObject cube = interactable.transform.gameObject;
        
            XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
                Debug.Log("Cube verrouillé");
            }

            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log("Rigidbody désactivé");
            }
            
            socket.enabled = false;
            Debug.Log("Socket désactivé");
        }
    }

    IEnumerator MoveTicket()
    {
        Debug.Log("Début du mouvement du ticket...");
        
        _startPosition = _socketMenu.transform.position;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _socketMenu.transform.position = Vector3.Lerp(
                _startPosition, 
                _positionEnd.position,
                elapsed / _duration
            );
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _socketMenu.transform.position = _positionEnd.position;
        Debug.Log("Ticket arrivé à destination !");
    }
}
