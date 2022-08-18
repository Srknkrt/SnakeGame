using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite snakeBodySprite;
    public Sprite snakeCornerSprite;
    public Sprite snakeTailSprite;
    public Sprite chestSprite;

    public static GameAssets i;

    private void Awake()
    {
        i = this;
    }
}
