using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleManager1 : MonoBehaviour
{
    public static PuzzleManager1 Instance;

    [Tooltip("Tous les SnapChecker du niveau")]
    public SnapChecker[] snapCheckers;

    [Header("Effets de victoire")]
    public AudioSource VictorySound;

    [Header("Animal à animer")]
    public Animator animalAnimator;     
    public GameObject animalObject;     
    public float escapeSpeed = 3f;      
    public float escapeDuration = 2.5f; 

    [Header("Transition vers les crédits")]
    public float delayBeforeCredits = 3f; // ← Durée avant de charger la scène des crédits
    public string creditsSceneName = "CreditsScene"; // ← Nom de ta scène des crédits (à adapter)

    private bool puzzleCompleted = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckAllSockets()
    {
        if (puzzleCompleted) return;

        foreach (var snap in snapCheckers)
        {
            if (!snap.IsCorrect) return; // si un snap est faux, on quitte
        }

        // Si on arrive ici, tout est correct
        OnPuzzleCompleted();
    }

    private void OnPuzzleCompleted()
    {
        puzzleCompleted = true;

        /*// Joue le son de victoire s’il existe
        if (VictorySound != null)
            VictorySound.Play();

        // Lance la fuite de l’animal
        if (animalAnimator != null)
            StartCoroutine(AnimalEscape());
        else
            StartCoroutine(WaitAndLoadCredits()); // si pas d’animal, on lance directement le délai
    }

    private IEnumerator AnimalEscape()
    {
        Debug.Log("🐾 L’animal prend la fuite !");

        // Lance l’animation "Run"
        animalAnimator.SetTrigger("Run");

        // Fait bouger l’animal vers l’avant pendant quelques secondes
        float timer = 0f;
        Vector3 direction = animalObject.transform.forward;

        while (timer < escapeDuration)
        {
            animalObject.transform.position += direction * escapeSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Fais disparaître l’animal après la fuite
        Destroy(animalObject);

        // Attends la fin du son de victoire (ou un délai fixe)
        yield return StartCoroutine(WaitAndLoadCredits());
    }

    private IEnumerator WaitAndLoadCredits()
    {
        // Attendre le son s’il existe
        float waitTime = delayBeforeCredits;

        if (VictorySound != null)
        {
            // On prend la durée du clip si elle est plus longue que le délai prévu
            float clipLength = VictorySound.clip != null ? VictorySound.clip.length : 0f;
            waitTime = Mathf.Max(delayBeforeCredits, clipLength);
        }

        Debug.Log($"⏳ Attente de {waitTime} secondes avant les crédits...");
        yield return new WaitForSeconds(waitTime);

        // Charge la scène des crédits
        Debug.Log("🎬 Chargement de la scène des crédits...");
        SceneManager.LoadScene(creditsSceneName);*/
    }
}

