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
    
    //Dialogue Chevalresse Epée Squelette
    public void LoadLevel1()
    {   _currentLevel = 2;
        LoadNewLevel(2);
        
    }
    
    //Dialogue Chevalresse Epée Roi
    public void LoadLevel2()
    {
        _currentLevel = 2;
        LoadNewLevel(3); 
        
    }
    
    //Dialogue Chevalresse Bouclier Roi
    public void LoadLevel3()
    {
        _currentLevel = 3;
        LoadNewLevel(4); 
        ;
    }
    
    //Dialogue Chevalresse Bouclier Squelette
    public void LoadLevel4()
    {
        _currentLevel = 3;
        LoadNewLevel(5); 
    }
    
    //Dialogue Squelette Epee Roi
    public void LoadLevel5()
    {
        _currentLevel = 3;
        LoadNewLevel(6); 
    }
    
    //Dialogue Squelette Epee Chevalresse
    public void LoadLevel6()
    {
        _currentLevel = 3;
        LoadNewLevel(7); 
    }
    
    //Dialogue Squelette Bouclier Roi
    public void LoadLevel7()
    {
        _currentLevel = 3;
        LoadNewLevel(8); 
    }
    
    //Dialogue Squelette Bouclier Chevalresse
    public void LoadLevel8()
    {
        _currentLevel = 3;
        LoadNewLevel(9); 
    }
    
    //Dialogue Roi Epée Chevalresse
    public void LoadLevel9()
    {
        _currentLevel = 3;
        LoadNewLevel(10); 
    }
    
    //Dialogue Roi Epée Squelette
    public void LoadLevel10()
    {
        _currentLevel = 3;
        LoadNewLevel(11); 
    }
    
    //Dialogue Roi Bouclier Chevalresse
    public void LoadLevel11()
    {
        _currentLevel = 3;
        LoadNewLevel(12); 
    }
    
    //Dialogue Roi Bouclier Squelette
    public void LoadLevel12()
    {
        _currentLevel = 3;
        LoadNewLevel(13); 
    }
    
    
    
    public void Quit()
    {
        Application.Quit();
    }
    
}
