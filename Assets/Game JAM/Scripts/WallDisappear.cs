using UnityEngine;

public class WallDisappear : MonoBehaviour
{
    [Header("Wall to Disable")]
    public GameObject wallObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (wallObject != null)
            {
                wallObject.SetActive(false);
                Debug.Log("Wall disappeared!");
            }
        }
    }
}
