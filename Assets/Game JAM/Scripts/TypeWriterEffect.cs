using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Le texte à afficher
    public float delay = 0.05f;      // Temps entre chaque lettre

    private string fullText;

    void OnEnable()
    {
        if (textMeshPro == null) return;

        fullText = textMeshPro.text; // Sauvegarde le texte complet
        textMeshPro.text = "";       // Vide le texte avant de l’afficher
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        foreach (char c in fullText)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
