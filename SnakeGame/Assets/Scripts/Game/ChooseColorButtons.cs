using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseColorButtons : MonoBehaviour
{
    public Button button;

    string buttonName;
    Snake snake;
    GameController gameController;

    public void ChangedSnakeColor()
    {
        buttonName = button.name.Split(' ')[0];
        PlayerPrefs.SetString("DefaultMaterial", buttonName);
        gameController = Object.FindObjectOfType<GameController>();
        gameController.SnakeDefaultColor();

        snake = Object.FindObjectOfType<Snake>();
        snake.ColorSnake();

    }
}
