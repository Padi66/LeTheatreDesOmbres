using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PuzzleCombination
{
    public string combinationName;
    public string[] requiredTags;
    public GameObject objectToSpawn;
    public Transform spawnLocation;
    public ParticleSystem fx;
}

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    [SerializeField] ParticleSystem _particleSystem;

    [Tooltip("Tous les SnapChecker du niveau")]
    public SnapChecker[] snapCheckers;

    [Header("Puzzle Combinations")]
    public PuzzleCombination[] combinations;

    private bool puzzleCompleted = false;

    private void Awake()
    {
        Instance = this;
        
    }

    public void CheckCombination()
    {
        if (puzzleCompleted) return;

        string[] currentTags = new string[snapCheckers.Length];
        for (int i = 0; i < snapCheckers.Length; i++)
        {
            currentTags[i] = snapCheckers[i].CurrentTag;
        }

        foreach (var combination in combinations)
        {
            if (MatchesCombination(currentTags, combination.requiredTags))
            {
                OnCombinationMatched(combination);
                return;
            }
        }
    }

    private bool MatchesCombination(string[] currentTags, string[] requiredTags)
    {
        if (currentTags.Length != requiredTags.Length) 
        {
            Debug.LogWarning($"Tag array length mismatch: {currentTags.Length} vs {requiredTags.Length}");
            return false;
        }

        for (int i = 0; i < currentTags.Length; i++)
        {
            if (requiredTags[i] == "" || requiredTags[i] == "Empty")
            {
                if (currentTags[i] != "")
                {
                    return false;
                }
            }
            else
            {
                if (currentTags[i] != requiredTags[i])
                {
                    return false;
                }
            }
        }

        Debug.Log("Combination matched! Checking if valid...");
        return true;
    }

    /*private void OnCombinationMatched(PuzzleCombination combination)
    {
        if (puzzleCompleted) return;
        
        puzzleCompleted = true;
        Debug.Log($"✓ Combination matched: {combination.combinationName}");

        if (combination.objectToSpawn != null && combination.spawnLocation != null)
        {
            GameObject spawned = Instantiate(combination.objectToSpawn, combination.spawnLocation.position, combination.spawnLocation.rotation);
            Debug.Log($"Spawned: {spawned.name}");
        }
        else
        {
            Debug.LogWarning("Cannot spawn - objectToSpawn or spawnLocation is null!");
        }
    }*/
    private void OnCombinationMatched(PuzzleCombination combination)
    {
        if (puzzleCompleted) return;

        puzzleCompleted = true;
        Debug.Log($"✓ Combination matched: {combination.combinationName}");

        // Destroy all blocks currently placed in each socket
        foreach (var snapChecker in snapCheckers)
        {
            if (snapChecker.CurrentObject != null)
            {
                Destroy(snapChecker.CurrentObject);
                snapChecker.CurrentObject = null;
            }
        }


        // Spawn object if needed
        if (combination.objectToSpawn != null && combination.spawnLocation != null)
        {
            GameObject spawned = Instantiate(combination.objectToSpawn,
                combination.spawnLocation.position,
                combination.spawnLocation.rotation);

            spawned.name = combination.objectToSpawn.name;
                
            ObjectResetter resetter = spawned.GetComponent<ObjectResetter>();
            if (resetter != null)
            {
                resetter.SetInitialTransform();
            }
            
            _particleSystem.Play();

            Debug.Log($"Spawned: {spawned.name}");
        }
        else
        {
            Debug.LogWarning("Cannot spawn - objectToSpawn or spawnLocation is null!");
        }
    }

}
