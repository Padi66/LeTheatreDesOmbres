using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class SceneTeleporter : MonoBehaviour
{
    [Header("Scene Settings")]
    public int sceneToLoadBuildIndex;
    public int dialogueBranchNumber = 1;

    [Header("Movement Providers")]
    public ContinuousMoveProvider continuousMoveProvider;
    public ContinuousTurnProvider continuousTurnProvider;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            SceneTransitionManager.TeleportToScene(sceneToLoadBuildIndex, dialogueBranchNumber, continuousMoveProvider, continuousTurnProvider);
        }
    }
}
