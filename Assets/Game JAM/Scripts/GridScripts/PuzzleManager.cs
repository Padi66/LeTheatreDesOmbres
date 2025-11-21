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
}

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

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

        if (!AllSocketsFilled()) return;

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

    private bool AllSocketsFilled()
    {
        foreach (var snap in snapCheckers)
        {
            if (!snap.HasObject) return false;
        }
        return true;
    }

    private bool MatchesCombination(string[] currentTags, string[] requiredTags)
    {
        if (currentTags.Length != requiredTags.Length) return false;

        for (int i = 0; i < currentTags.Length; i++)
        {
            if (currentTags[i] != requiredTags[i]) return false;
        }
        return true;
    }

    private void OnCombinationMatched(PuzzleCombination combination)
    {
        puzzleCompleted = true;
        Debug.Log($"Combination matched: {combination.combinationName}");

        if (combination.objectToSpawn != null && combination.spawnLocation != null)
        {
            Instantiate(combination.objectToSpawn, combination.spawnLocation.position, combination.spawnLocation.rotation);
        }
    }
}
