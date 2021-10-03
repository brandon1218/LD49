using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleCanvas : MonoBehaviour
{
    public bool changingScene;
    public GameObject changingSceneMask;
    public GameObject gameTitleText;
    public GameObject playButton;

    public Text levelIndexText;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void StartButtonClick()
    {
        changingScene = true;
        levelIndexText.text = "level - 1";
        Invoke("ChangeToGameScene", 1);
    }

    void ChangeToGameScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
        changingScene = false;

        gameTitleText.transform.position = new Vector3(Screen.width * 2, gameTitleText.transform.position.y, 0);
        playButton.transform.position = new Vector3(Screen.width * 2, playButton.transform.position.y, 0);
    }

    private void Update()
    {
        if (changingScene)
        {
            changingSceneMask.transform.position = Vector3.Lerp(changingSceneMask.transform.position, new Vector3(Screen.width/2, Screen.height/2, 0), 0.05f);
        }
        else
        {
            changingSceneMask.transform.position = Vector3.Lerp(changingSceneMask.transform.position, new Vector3(Screen.width / 2, Screen.height * 2, 0), 0.05f);
        }
    }



}
