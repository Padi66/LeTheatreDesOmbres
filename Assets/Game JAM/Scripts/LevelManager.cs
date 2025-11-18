using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int _currentLevel;

    public void LoadNewLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        
    }
    
    
    public void LoadLevel1()
    {
        LoadNewLevel(1);
        _currentLevel = 1;

    }
    public void LoadLevel2()
    {
        _currentLevel = 2;
        LoadNewLevel(2); 
        
       
        
    }
    public void LoadLevel3()
    {
        _currentLevel = 3;
        LoadNewLevel(2); 
        
       
        
    }
    
}
