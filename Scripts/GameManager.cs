using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    Player player;
    public GameObject gameFailPanel;
    public GameObject gameSuccessPanel;

    bool gameFail;
    public bool gameSuccess;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToTitleScene();
        }
        if (player.transform.position.y <= -5)
        {
            if (!gameFail)
            {
                if (!FindObjectOfType<TitleCanvas>().changingScene)
                {
                    gameFail = true;
                }
               
            }
        }

        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        if (characters.Length == 1)
        {
            if (characters[0].GetComponent<Player>())
            {
                if (!gameSuccess)
                {
                    if (!FindObjectOfType<TitleCanvas>().changingScene)
                    {
                        if (!gameFail)
                        {
                        gameSuccess = true;

                        }
                    }

                  
                }
            }
        }



        if (gameFail)
        {
            gameFailPanel.transform.position = Vector3.Lerp(gameFailPanel.transform.position, new Vector3(Screen.width /2, Screen.height/2, 0), 0.1f);
        }
        else
        {
            gameFailPanel.transform.position = Vector3.Lerp(gameFailPanel.transform.position, new Vector3(Screen.width / 2, -Screen.height * 2, 0), 0.1f);
        }
        if (gameSuccess)
        {
            gameSuccessPanel.transform.position = Vector3.Lerp(gameSuccessPanel.transform.position, new Vector3(Screen.width / 2, Screen.height / 2, 0), 0.1f);
        }
        else
        {
            gameSuccessPanel.transform.position = Vector3.Lerp(gameSuccessPanel.transform.position, new Vector3(Screen.width / 2, -Screen.height * 2, 0), 0.1f);
        }

    }

    public void RestartButtonClick()
    {
        Invoke("ReloadScene", 1);
        FindObjectOfType<TitleCanvas>().changingScene = true;


        gameFail = false;
    }

    public void QuitButtonClick()
    {
        BackToTitleScene();
    }

    public void NextLevelButtonClick()
    {
        Invoke("LoadNextLevel", 1);
        FindObjectOfType<TitleCanvas>().changingScene = true;

        Scene currentScene = SceneManager.GetActiveScene();
        FindObjectOfType<TitleCanvas>().levelIndexText.text = "level - " +(currentScene.buildIndex+1).ToString() ;
        gameSuccess = false;
    }

    void LoadNextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
        FindObjectOfType<TitleCanvas>().changingScene = false;
    }
    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        FindObjectOfType<TitleCanvas>().changingScene = false;
    }

    void BackToTitleScene()
    {
        SceneManager.LoadScene("Title");
        Destroy(FindObjectOfType<BgmPlayer>().gameObject);
    }
   

}
