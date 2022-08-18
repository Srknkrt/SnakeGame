using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    string levelNumberText;
    int lNumber;

    private void Start()
    {
        /*for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("Level" + i, 0);
        }*/


        lNumber = FindLevelActiveLevelNumber();

        text.text = "Level " + lNumber + "\n";

        for (int j = 0; j < PlayerPrefs.GetInt("Level" + lNumber.ToString()); j++)
        {
            text.text += " *";
        }

        button.enabled = false;

        if(lNumber == 1 || PlayerPrefs.GetInt("Level" + (lNumber - 1).ToString()) > 0)
        {
            button.enabled = true;
        }
    }

    private int FindLevelActiveLevelNumber()
    {
        levelNumberText = text.text;

        levelNumberText = levelNumberText.Split(' ')[1];

        lNumber = int.Parse(levelNumberText);

        return lNumber;
    }

    public void PlayBtn()
    {
        lNumber = FindLevelActiveLevelNumber();

        PlayerPrefs.SetString("ActiveLevel",lNumber.ToString());
        SceneManager.LoadScene("Scenes/GameScene");
    }
}
