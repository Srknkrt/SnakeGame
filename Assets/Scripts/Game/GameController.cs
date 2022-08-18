using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Image timeImage;
    [SerializeField] private Image timeBackgroundImage;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image scoreImage;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI infoText;



    [SerializeField] private Image featureImage;
    public TextMeshProUGUI activationText;

    [SerializeField] private List<Sprite> featureSprites;

    [SerializeField] private ParticleSystem cloudy;


    public Material invisibleMaterial;

    public Material greenMaterial;
    public Material blueMaterial;
    public Material purpleMaterial;
    public Material redMaterial;
    public Material yellowMaterial;

    public Material defaultMaterial;


    public List<GameObject> leftWalls;
    public List<GameObject> rightWalls;
    public List<GameObject> upWalls;
    public List<GameObject> downWalls;

    float currentTime;
    float duration;
    float limitScore;
    string activeLevel;

    Snake snake;

    private void Awake()
    {
        SnakeDefaultColor();
    }

    enum Level
    {
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten
    }

    Level level;


    void Start()
    {
        snake = Object.FindObjectOfType<Snake>();

        limitScore = snake.passingScore;
        duration = limitScore / 20;
        currentTime = 0f;
        timeText.text = "0";
        scoreText.text = "0";
        featureImage.gameObject.SetActive(false);
        activationText.gameObject.SetActive(false);

        activeLevel = PlayerPrefs.GetString("ActiveLevel");
        DestroyWalls();
        FindActiveLevel();
        PlaceWalls();

        StartCoroutine(CountdownTime());
    }

    public void EatInfoText()
    {
        infoText.text = "Go to Apple";
    }

    public void ChestInfoText()
    {
        infoText.text = "Go to Chest";
    }

    public void FeatureInfoText(Snake.Feature feature)
    {
        switch (feature)
        {
            case Snake.Feature.ExtraPoints:
                infoText.text = "Extra Points: When activated, you get 2x more points than apples.";
                break;
            case Snake.Feature.SpeedUp:
                infoText.text = "Speed Up: When activated, your speed is increased by 2 times.";
                break;
            case Snake.Feature.SpeedDown:
                infoText.text = "Speed Down: When activated, your speed is reduced by 2 times.";
                break;
            case Snake.Feature.Cloudy:
                infoText.text = "Cloudly: When activated, clouds pass through the sky.";
                break;
            case Snake.Feature.BodyInvisibility:
                infoText.text = "BodyInvisibility: When activated, all parts of the snake are invisible except for its head and tail.";
                break;
            case Snake.Feature.Dizziness:
                infoText.text = "Dizziness: When activated, the snake begins to move in the opposite direction.";
                break;
        }
    }

    public void GameOverInfoText()
    {
        infoText.text = "Game Over!";
    }

    public void StopInfoText()
    {
        infoText.text = "Press the arrow keys to continue the game.";
    }

    public void LevelComptleteInfoText(string starCount)
    {
        if(starCount == "1")
        {
            infoText.text = "Congratulations, you managed to complete the chapter on time. The next chapter is unlocked.";
        }

        if(starCount == "2")
        {
            infoText.text = "Congratulations, you have completed the section with 2 stars.";
        }

        if(starCount == "3")
        {
            infoText.text = "Congratulations, you have completed the section with 3 stars.";
        }
    }

    IEnumerator CountdownTime()
    {
        while(snake.state != Snake.State.Dead)
        {
            if(duration >= currentTime)
            {
                timeImage.fillAmount = Mathf.InverseLerp(duration, 0, currentTime);
            }
            else if(duration * 2 >= currentTime)
            {
                timeBackgroundImage.fillAmount = Mathf.InverseLerp(duration * 2, duration, currentTime);
            }
            timeText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime++;
        }
        yield return null;
    }

    public IEnumerator ActivationTimer(float timer, float activeTime)
    {
        activationText.gameObject.SetActive(true);
        while (timer > 0)
        {
            activationText.color = Color.black;
            activationText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }
        while (activeTime > 0)
        {
            activationText.color = Color.red;
            activationText.text = activeTime.ToString();
            yield return new WaitForSeconds(1f);
            activeTime--;
        }
        activationText.gameObject.SetActive(false);
    }

    public void FeatureChanged(Snake.Feature feature)
    {
        featureImage.gameObject.SetActive(true);
        switch (feature)
        {
            case Snake.Feature.ExtraPoints:
                featureImage.sprite = featureSprites[0];
                break;
            case Snake.Feature.SpeedUp:
                featureImage.sprite = featureSprites[1];
                break;
            case Snake.Feature.SpeedDown:
                featureImage.sprite = featureSprites[2];
                break;
            case Snake.Feature.Dizziness:
                featureImage.sprite = featureSprites[3];
                break;
            case Snake.Feature.Cloudy:
                featureImage.sprite = featureSprites[4];
                break;
            case Snake.Feature.BodyInvisibility:
                featureImage.sprite = featureSprites[5];
                break;
        }
    }

    public void FeatureImageReset()
    {
        featureImage.gameObject.SetActive(false);
    }

    public void PlayCloudly()
    {
        cloudy.Play();
    }


    public void PrintToScoreText(float score)
    {
        scoreImage.fillAmount = Mathf.InverseLerp(0, limitScore, score);
        
        if (PlayerPrefs.GetInt("Level" + activeLevel) != 3 && score == limitScore)
        {
            if(currentTime < duration)
            {
                PlayerPrefs.SetInt("Level" + activeLevel, 3);
                LevelComptleteInfoText("3");
            }
            else if(currentTime < duration * 2)
            {
                PlayerPrefs.SetInt("Level" + activeLevel, 2);
                LevelComptleteInfoText("2");
            }
        }
        scoreText.text = score.ToString();
    }

    void FindActiveLevel()
    {
        switch (activeLevel)
        {
            case "1":
                level = Level.one;
                break;
            case "2":
                level = Level.two;
                break;
            case "3":
                level = Level.three;
                break;
            case "4":
                level = Level.four;
                break;
            case "5":
                level = Level.five;
                break;
            case "6":
                level = Level.six;
                break;
            case "7":
                level = Level.seven;
                break;
            case "8":
                level = Level.eight;
                break;
            case "9":
                level = Level.nine;
                break;
            case "10":
                level = Level.ten;
                break;
        }
    }

    void PlaceWalls()
    {
        switch (level)
        {
            case Level.one:
                PlaceLevel1Wall();
                break;
            case Level.two:
                PlaceLevel2Wall();
                break;
            case Level.three:
                PlaceLevel3Wall();
                break;
            case Level.four:
                PlaceLevel4Wall();
                break;
            case Level.five:
                PlaceLevel5Wall();
                break;
            case Level.six:
                PlaceLevel6Wall();
                break;
            case Level.seven:
                PlaceLevel7Wall();
                break;
            case Level.eight:
                PlaceLevel8Wall();
                break;
            case Level.nine:
                PlaceLevel9Wall();
                break;
            case Level.ten:
                PlaceLevel10Wall();
                break;
        }
    }

    void PlaceLevel1Wall()
    {

    }

    void PlaceLevel2Wall()
    {
        for (int i = 0; i < 4; i++)
        {
            leftWalls[i].gameObject.SetActive(true);
            leftWalls[leftWalls.Count - i - 1].gameObject.SetActive(true);
            rightWalls[i].gameObject.SetActive(true);
            rightWalls[rightWalls.Count - i - 1].gameObject.SetActive(true);

            if (i < 3)
            {
                upWalls[i].gameObject.SetActive(true);
                upWalls[upWalls.Count - i - 1].gameObject.SetActive(true);

                downWalls[i].gameObject.SetActive(true);
                downWalls[downWalls.Count - i - 1].gameObject.SetActive(true);
            }
        }
    }

    void PlaceLevel3Wall()
    {
        for (int i = 3; i < 16; i++)
        {
            if (i > 3)
            {
                leftWalls[i].gameObject.SetActive(true);
                rightWalls[i].gameObject.SetActive(true);
            }
            if (i < 15)
            {
                upWalls[i].gameObject.SetActive(true);
                downWalls[i].gameObject.SetActive(true);
            }
        }
    }

    void PlaceLevel4Wall()
    {
        for (int i = 0; i < 18; i++)
        {
            upWalls[i].gameObject.SetActive(true);
            downWalls[i].gameObject.SetActive(true);
        }
        leftWalls[0].gameObject.SetActive(true);
        leftWalls[leftWalls.Count - 1].gameObject.SetActive(true);
        rightWalls[0].gameObject.SetActive(true);
        rightWalls[leftWalls.Count - 1].gameObject.SetActive(true);
    }

    void PlaceLevel5Wall()
    {
        for (int i = 0; i < 20; i++)
        {
            leftWalls[i].gameObject.SetActive(true);
            rightWalls[i].gameObject.SetActive(true);
        }
    }

    void PlaceLevel6Wall()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i % 8 < 4)
            {
                leftWalls[i].gameObject.SetActive(true);
                rightWalls[i].gameObject.SetActive(true);
            }
            if (i % 4 >= 2 && i < 18)
            {
                upWalls[i].gameObject.SetActive(true);
                downWalls[i].gameObject.SetActive(true);
            }
        }
    }

    void PlaceLevel7Wall()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i % 4 < 2)
            {
                leftWalls[i].gameObject.SetActive(true);
            }
            else
            {
                rightWalls[i].gameObject.SetActive(true);
            }
        }
    }

    void PlaceLevel8Wall()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i < 10)
            {
                leftWalls[i].gameObject.SetActive(true);
            }
            else if (i > 10)
            {
                rightWalls[i].gameObject.SetActive(true);
            }
            if (i < 9 && i < 18)
            {
                upWalls[i].gameObject.SetActive(true);
            }
            else if (i > 9 && i < 18)
            {
                downWalls[i].gameObject.SetActive(true);
            }
        }
    }

    void PlaceLevel9Wall()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i % 2 == 0)
            {
                leftWalls[i].gameObject.SetActive(true);
                rightWalls[i].gameObject.SetActive(true);
            }
            else if (i % 2 == 1 && i < 18)
            {
                upWalls[i].gameObject.SetActive(true);
                downWalls[i].gameObject.SetActive(true);
            }

        }
        leftWalls[leftWalls.Count - 1].gameObject.SetActive(true);
        rightWalls[rightWalls.Count - 1].gameObject.SetActive(true);
    }

    void PlaceLevel10Wall()
    {
        for (int i = 0; i < 20; i++)
        {
            leftWalls[i].gameObject.SetActive(true);
            rightWalls[i].gameObject.SetActive(true);
            if (i < 18)
            {
                upWalls[i].gameObject.SetActive(true);
                downWalls[i].gameObject.SetActive(true);
            }
        }
    }

    void DestroyWalls()
    {
        for (int i = 0; i < 20; i++)
        {
            leftWalls[i].gameObject.SetActive(false);
            rightWalls[i].gameObject.SetActive(false);
            if (i < 18)
            {
                upWalls[i].gameObject.SetActive(false);
                downWalls[i].gameObject.SetActive(false);
            }
        }
    }

    public void SnakeDefaultColor()
    {
        if (PlayerPrefs.GetString("DefaultMaterial") == "")
        {
            defaultMaterial = greenMaterial;
            PlayerPrefs.SetString("DefaultMaterial", "Green");
        }
        if (PlayerPrefs.GetString("DefaultMaterial") == "Green")
        {
            defaultMaterial = greenMaterial;
        }
        if (PlayerPrefs.GetString("DefaultMaterial") == "Blue")
        {
            defaultMaterial = blueMaterial;
        }
        if (PlayerPrefs.GetString("DefaultMaterial") == "Purple")
        {
            defaultMaterial = purpleMaterial;
        }
        if (PlayerPrefs.GetString("DefaultMaterial") == "Red")
        {
            defaultMaterial = redMaterial;
        }
        if (PlayerPrefs.GetString("DefaultMaterial") == "Yellow")
        {
            defaultMaterial = yellowMaterial;
        }
    }

}
