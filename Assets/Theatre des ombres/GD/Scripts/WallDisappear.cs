using UnityEngine;

public class WallDisappear : MonoBehaviour
{
    [Header("Wall to Disable")]
    public GameObject wallObject;

    [Header("Teleport Zone to Activate")]
    public GameObject teleportZoneToActivate;

    [Header("Destination Settings")]
    public int sceneToLoad;
    public int dialogueBranchToShow;

    [Header("Other Triggers to Disable")]
    public GameObject[] otherTriggersToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (wallObject != null)
            {
                wallObject.SetActive(false);
                Debug.Log("Wall disappeared!");
            }

            if (teleportZoneToActivate != null)
            {
                SceneTeleporter teleporter = teleportZoneToActivate.GetComponent<SceneTeleporter>();
                if (teleporter != null)
                {
                    teleporter.sceneToLoadBuildIndex = sceneToLoad;
                    teleporter.dialogueBranchNumber = dialogueBranchToShow;
                    Debug.Log($"Teleporter configured: Scene {sceneToLoad}, Dialogue Branch {dialogueBranchToShow}");
                }

                teleportZoneToActivate.SetActive(true);
                Debug.Log("Teleport zone activated!");
            }

            DisableAllTriggers();
        }
    }

    private void DisableAllTriggers()
    {
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} disabled itself");

        if (otherTriggersToDisable != null)
        {
            foreach (GameObject trigger in otherTriggersToDisable)
            {
                if (trigger != null)
                {
                    trigger.SetActive(false);
                    Debug.Log($"{trigger.name} disabled");
                }
            }
        }
    }
}
