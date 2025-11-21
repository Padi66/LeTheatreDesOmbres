using UnityEngine;

public class ObjectResetter : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    public Vector3 InitialPosition => _initialPosition;
    public Quaternion InitialRotation => _initialRotation;

    void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }
}