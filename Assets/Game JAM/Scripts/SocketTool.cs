
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketTool : MonoBehaviour
{
    [SerializeField] private Light _lightcolor;
    [SerializeField] private XRSocketInteractor _socketInteractor;
    

    void OnEnable()
    {
        _socketInteractor.selectEntered.AddListener(OnSelectEntered);
        _socketInteractor.selectExited.AddListener(OnSelectExited);
    }
    void OnDisable()
    {
        _socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        _socketInteractor.selectExited.RemoveListener(OnSelectExited);
    }
    
    

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        //if (args.interactableObject.transform.gameObject.GetComponent<CubeOrange>())
       
        GameObject cube = args.interactableObject.transform.gameObject;
        string _cubeName = cube.name;
        _lightcolor.intensity = 1;
        
        if (cube.GetComponent<Sword>())
        {
            Debug.Log("Socket Tool contient Sword");
        }
        else if (cube.GetComponent<Shield>())
        {
            Debug.Log("Socket Tool contient Shield");
        }
        
        StoryManager.OnSocketStateChanged?.Invoke("Tool", true);
        StoryManager.OnCubePlaced?.Invoke("Tool", _cubeName);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        _lightcolor.intensity = 0;
        Debug.Log("Socket Tool vide");
        StoryManager.OnSocketStateChanged?.Invoke("Tool", false);
        StoryManager.OnCubePlaced?.Invoke("Tool", null);
    }
}

