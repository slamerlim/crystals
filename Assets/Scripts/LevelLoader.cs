using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public Animator transition;

    public float transitionTime = 2f;

    public Text levelName;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");


        if (levelIndex == 1)
        {
            levelName.text = "Level 1: the entrance";
        }
        else if (levelIndex == 2)
        {
            levelName.text = "Level 2: the shaft";
        }
        else if (levelIndex == 3)
        {
            levelName.text = "Level 3: the only path";
        }
        else if (levelIndex == 4)
        {
            levelName.text = "to be continued...";
        }
        else if (levelIndex == 5)
        {
            levelIndex = 1;
            levelName.text = "Level 1: the entrance";
        }

        //Wait for animation to stop
        yield return new WaitForSeconds(transitionTime);


        //Load next level
        SceneManager.LoadScene(levelIndex);
    }
}
