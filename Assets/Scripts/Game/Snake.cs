using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // The aspects of the character were created.
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Stop
    }
    // Character's states are created.
    public enum State
    {
        Alive,
        Dead
    }

    public enum Feature
    {
        ExtraPoints,
        SpeedUp,
        SpeedDown,
        Cloudy,
        BodyInvisibility,
        Dizziness
    }

    [SerializeField] private GameObject snake;


    public Vector2Int chestGridPosition;
    GameObject chestGameObject;
    
    public State state;
    public float passingScore;
    bool chestAlive;

    float chestSpawnTime = 20f;
    float chestDeathTime = 10f;
    float activationTime = 3f;


    public Direction gridMoveDirection;
    public Direction oldGridMoveDirection;
    public bool canIt;

    public bool dizziness;

    Vector2Int gridPosition;
    
    float speed;
    float multiplier;

    float score;
    float coefficient;

    int snakeBodySize;

    GameController gameController;
    SoundController soundController;
    LevelGrid levelGrid;

    List<SnakeMovePosition> snakeMovePositionList;
    List<SnakeBodyPart> snakeBodyPartList;

    // The codes that will work before the start of the game have been created.
    void Awake()
    {
        gameController = Object.FindObjectOfType<GameController>();
        soundController = Object.FindObjectOfType<SoundController>();

        // Default direction assigned.
        gridMoveDirection = (Direction)Random.Range(0, 4);
        oldGridMoveDirection = gridMoveDirection;

        canIt = true;
        // The default state has been assigned.
        state = State.Alive;
        // The starting position has been assigned.
        gridPosition = new Vector2Int(Random.Range(5, 10), Random.Range(5, 10));
        // A value is assigned to the variable that generates the speed.
        speed = 9f;
        multiplier = 1f;

        // The list to which the snake's positions will be assigned has been created.
        snakeMovePositionList = new List<SnakeMovePosition>();
        // The initial size of the snake has been created.
        snakeBodySize = 1;
        // A list was created to keep the body parts of the snake.
        snakeBodyPartList = new List<SnakeBodyPart>();

        dizziness = false;

        score = 0f;
        coefficient = 1f;

        gameController.EatInfoText();
        gameController.PrintToScoreText(score);

        CreateSnakeTail();
    }

    // Codes that will run at the start of the game have been created.
    void Start()
    {
        // FPS value assigned.
        Application.targetFrameRate = 90;
        snake.GetComponent<SpriteRenderer>().material = gameController.defaultMaterial;
        
        Time.timeScale = 1;
        
        passingScore = 5000f;
        chestAlive = false;
        InvokeRepeating("SpawnChest", chestSpawnTime, chestSpawnTime);

        StartCoroutine(HandleGridMovement());
    }

    void Update()
    {
        if (state == State.Dead)
        {
            gameController.GameOverInfoText();
            soundController.MusicStop();
            BodyVisible();
            Time.timeScale = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            state = State.Dead;
        }
    }

    void CreateSnakeTail()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    IEnumerator HandleGridMovement()
    {
        
        while (state == State.Alive)
        {
            yield return new WaitForSeconds(1 / (speed * multiplier));
            // Written code to find the previous position of the snake.
            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            // Created the code that writes the snake's position to the list.
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            // The code that enables the movement of the snake has been created.
            Vector2Int gridMoveDirectionVector = new Vector2Int(gridPosition.x, gridPosition.y);
            switch (gridMoveDirection)
            {
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, 1);
                    break;
                case Direction.Down:
                    gridMoveDirectionVector = new Vector2Int(0, -1);
                    break;
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-1, 0);
                    break;
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2Int(1, 0);
                    break;
                case Direction.Stop:
                    break;
            }
            gridPosition += gridMoveDirectionVector;
            canIt = true;
            // The code that keeps the snake on the playground has been written.
            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            // It was checked whether the snake ate the food.
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);

            if (snakeAteFood)
            {
                // Snake ate food, grow body
                snakeBodySize++;
                RaiseScore();
                CreateSnakeBody();
            }

            DidItPassLevel();

            if (chestAlive)
            {
                TrySnakeTakeChest(gridPosition);
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
            transform.localScale = Vector3.one;

            if (gridMoveDirection != Direction.Stop)
            {
                // The snake was made to follow its body.
                UpdateSnakeBodyParts();
            }

            // It was checked whether the snake bit its own body or not.
            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    state = State.Dead;
                }
            }
        }
    }

    void DidItPassLevel()
    {
        if ((score >= passingScore) && (PlayerPrefs.GetInt("Level" + PlayerPrefs.GetString("ActiveLevel")) == 0))
        {
            PlayerPrefs.SetInt("Level" + PlayerPrefs.GetString("ActiveLevel"), 1);
            gameController.LevelComptleteInfoText("1");
        }
    }

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    void RaiseScore()
    {
        score += 100f * coefficient;
        if(score % 1000 == 0 && score <= passingScore)
        {
            speed++;
        }

        IsBestScore(score);
        soundController.EatSoundPlay();
        gameController.PrintToScoreText(score);
    }

    void IsBestScore(float score)
    {
        float oldScore = PlayerPrefs.GetFloat("BestScore");
        if (oldScore < score)
        {
            PlayerPrefs.SetFloat("BestScore", score);
        }
    }

    void SpawnChest()
    {
        gameController.ChestInfoText();
        do
        {
            chestGridPosition = new Vector2Int(Random.Range(1, levelGrid.width - 1), Random.Range(1, levelGrid.height - 1));
        }
        while (GetFullSnakeGridPositionList().IndexOf(chestGridPosition) != -1 || levelGrid.foodGridPosition == chestGridPosition);
        chestAlive = true;
        chestGameObject = new GameObject("Chest", typeof(SpriteRenderer));
        chestGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.chestSprite;
        chestGameObject.transform.position = new Vector3(chestGridPosition.x, chestGridPosition.y);
        StartCoroutine(ChestDeathTimer(chestDeathTime));
    }

    IEnumerator ChestDeathTimer(float timer)
    {
        while (timer > 0 && chestAlive)
        {
            yield return new WaitForSeconds(1f);
            timer--;
        }
        if (chestAlive)
        {
            DestroyChest();
            gameController.EatInfoText();
        }
    }

    void TrySnakeTakeChest(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == chestGridPosition)
        {
            soundController.ChestSoundPlay();
            DestroyChest();
            EnableFeature();
        }
    }


    void DestroyChest()
    {
        chestAlive = false;
        Object.Destroy(chestGameObject);
    }

    void EnableFeature()
    {
        Feature feature = (Feature)Random.Range(0, 6);
        StartCoroutine(gameController.ActivationTimer(activationTime, chestSpawnTime - chestDeathTime - activationTime));
        gameController.FeatureInfoText(feature);
        gameController.FeatureChanged(feature);
        switch (feature)
        {
            case Feature.ExtraPoints:
                Invoke("ExtraPoints", activationTime);
                break;
            case Feature.SpeedUp:
                Invoke("SpeedUp", activationTime);
                break;
            case Feature.SpeedDown:
                Invoke("SpeedDown", activationTime);
                break;
            case Feature.Cloudy:
                Invoke("Cloudy", activationTime);
                break;
            case Feature.BodyInvisibility:
                Invoke("BodyInvisibility", activationTime);
                break;
            case Feature.Dizziness:
                Invoke("Dizziness", activationTime);
                break;
        }
    }

    void ExtraPoints()
    {
        coefficient = 2f;
        Invoke("FeatureReset", chestSpawnTime - chestDeathTime - activationTime);
    }

    void SpeedUp()
    {
        multiplier = 2f;
        Invoke("FeatureReset", chestSpawnTime - chestDeathTime - activationTime);
    }

    void SpeedDown()
    {
        multiplier = 0.5f;
        Invoke("FeatureReset", chestSpawnTime - chestDeathTime - activationTime);
    }

    void Cloudy()
    {
        gameController.PlayCloudly();
        Invoke("FeatureReset", chestSpawnTime - chestDeathTime - activationTime);
    }

    void Dizziness()
    {
        dizziness = true;
        Invoke("FeatureReset", chestSpawnTime - chestDeathTime - activationTime);
    }

    void BodyVisible()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeVisible();
        }
    }

    void BodyInvisibility()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeInvisible((i == snakeBodyPartList.Count - 1));
        }
        Invoke("FeatureReset", chestSpawnTime - chestDeathTime - activationTime);
    }

    void FeatureReset()
    {
        coefficient = 1f;
        multiplier = 1f;
        dizziness = false;
        gameController.EatInfoText();
        BodyVisible();
        gameController.FeatureImageReset();
    }

    void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i], (i == snakeBodyPartList.Count - 1));
        }
    }

    public void ColorSnake()
    {
        snake.GetComponent<SpriteRenderer>().material = gameController.defaultMaterial;
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].ChangedSnakeColor();
        }
    }

    float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    // Return the full list of positions occupied by the snake: Head + Body
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    class SnakeBodyPart
    {
        SnakeMovePosition snakeMovePosition;
        Transform transform;
        GameController gameController;

        public SnakeBodyPart(int bodyIndex)
        {
            gameController = Object.FindObjectOfType<GameController>();
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));

            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            gameController.SnakeDefaultColor();
            snakeBodyGameObject.GetComponent<SpriteRenderer>().material = gameController.defaultMaterial;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeInvisible(bool isTail)
        {
            if (!isTail)
            {
                transform.GetComponent<SpriteRenderer>().material = gameController.invisibleMaterial;
            }
        }

        public void ChangedSnakeColor()
        {
            transform.GetComponent<SpriteRenderer>().material = gameController.defaultMaterial;
        }

        public void SetSnakeVisible()
        {
            transform.GetComponent<SpriteRenderer>().material = gameController.defaultMaterial;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition, bool isTail)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up: // Currently going to the Up
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                            }
                            else
                            {
                                SetSnakeBodySprite();
                            }
                            angle = 0;
                            break;
                        case Direction.Left:// Previously was going Left
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = 0;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = -90;
                            }
                            break;
                        case Direction.Right:// Previously was going Right
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = 0;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = 0;
                            }
                            break;
                    }
                    break;
                case Direction.Down: // Currently going to the Down
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                            }
                            else
                            {
                                SetSnakeBodySprite();
                            }
                            angle = 180;
                            break;
                        case Direction.Left:// Previously was going Left
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = 180;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = 180;
                            }
                            break;
                        case Direction.Right:// Previously was going Right
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = 180;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = 90;
                            }
                            break;
                    }
                    break;
                case Direction.Left: // Currently going to the Left
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                            }
                            else
                            {
                                SetSnakeBodySprite();
                            }
                            angle = 90;
                            break;
                        case Direction.Down:// Previously was going Down
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = 90;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = 0;
                            }
                            break;
                        case Direction.Up:// Previously was going Up
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = 90;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = 90;
                            }
                            break;
                    }
                    break;
                case Direction.Right: // Currently going to the Right
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                            }
                            else
                            {
                                SetSnakeBodySprite();
                            }
                            angle = -90;
                            break;
                        case Direction.Down:// Previously was going Down
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = -90;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = -90;
                            }
                            break;
                        case Direction.Up:// Previously was going Up
                            if (isTail)
                            {
                                SetSnakeTailSprite();
                                angle = -90;
                            }
                            else
                            {
                                SetSnakeCornerSprite();
                                angle = 180;
                            }
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }

        void SetSnakeTailSprite()
        {
            transform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeTailSprite;
        }

        void SetSnakeCornerSprite()
        {
            transform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeCornerSprite;
        }

        void SetSnakeBodySprite()
        {
            transform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
        }
    }

    //Handles one Move Position from the Snake
    class SnakeMovePosition
    {
        SnakeMovePosition previousSnakeMovePosition;
        Vector2Int gridPosition;
        Direction direction;
        
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if(previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;

            }
        }
    }
}