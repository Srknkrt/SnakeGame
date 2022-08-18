using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public Image pausedPanel;

    public TextMeshProUGUI bestScoreText;

    float bestScore;
    Snake snake;
    GameController gameController;

    private void Start()
    {
        snake = Object.FindObjectOfType<Snake>();
        gameController = Object.FindObjectOfType<GameController>();
    }

    public void PausedButton()
    {
        if (pausedPanel.gameObject.activeSelf)
        {
            snake.gridMoveDirection = Snake.Direction.Stop;

            gameController.EatInfoText();
            pausedPanel.gameObject.SetActive(false);
        }
        else
        {
            bestScore = PlayerPrefs.GetFloat("BestScore");
            bestScoreText.text = "You are best score: " + bestScore.ToString();
            
            gameController.StopInfoText();
            pausedPanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void HomeBtn()
    {
        SceneManager.LoadScene("Scenes/LevelMenuScene");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}
