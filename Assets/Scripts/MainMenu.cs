using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    private GameObject waitText;
    public LevelLoader levelLoader;

    void Start()
    {
        waitText = GameObject.FindGameObjectWithTag("WaitText");
        waitText.SetActive(false);
    }
    
    public void StartGame()
    {
        waitText.SetActive(true);
        StartCoroutine(LoadSceneAfterWait(2f));
        
    }
    
    IEnumerator LoadSceneAfterWait(float duration)
    {

        yield return new WaitForSeconds(duration);

        levelLoader.LoadNextLevel();
    }
    public void LeaveGame()
    {
        Application.Quit();
      
    }
}
