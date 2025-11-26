using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int _currentLevel;
    [SerializeField] DialogueSequence _dialogueSequence;

    public void LoadNewLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadMainMenu()
    {
        _currentLevel = 0;
        LoadNewLevel(0);
    }

    public void LoadBackStage()
    {
        _currentLevel = 1;
        LoadNewLevel(1);
    }

    public void LoadLevel1()
    {
        _currentLevel = 2;
        LoadNewLevel(2);
    }

    public void LoadLevel2()
    {
        _currentLevel = 2;
        LoadNewLevel(3);
    }

    public void LoadLevel3()
    {
        _currentLevel = 3;
        LoadNewLevel(4);
    }

    public void LoadLevel4()
    {
        _currentLevel = 3;
        LoadNewLevel(5);
    }

    public void LoadLevel5()
    {
        _currentLevel = 3;
        LoadNewLevel(6);
    }

    public void LoadLevel6()
    {
        _currentLevel = 3;
        LoadNewLevel(7);
    }

    public void LoadLevel7()
    {
        _currentLevel = 3;
        LoadNewLevel(8);
    }

    public void LoadLevel8()
    {
        _currentLevel = 3;
        LoadNewLevel(9);
    }

    public void LoadLevel9()
    {
        _currentLevel = 3;
        LoadNewLevel(10);
    }

    public void LoadLevel10()
    {
        _currentLevel = 3;
        LoadNewLevel(11);
    }

    public void LoadLevel11()
    {
        _currentLevel = 3;
        LoadNewLevel(12);
    }

    public void LoadLevel12()
    {
        _currentLevel = 3;
        LoadNewLevel(13);
    }

    public void Quit()
    {
        Debug.Log("Application.Quit() appelé - fonctionnera seulement en BUILD, pas dans l'éditeur!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
