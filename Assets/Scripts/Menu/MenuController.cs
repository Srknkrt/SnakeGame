using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void SinglePlayerBtn()
    {
        SceneManager.LoadScene("Scenes/LevelMenuScene");
    }

    public void MultiPlayerBtn()
    {
        SceneManager.LoadScene("Scenes/MultiLobbyScene");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
