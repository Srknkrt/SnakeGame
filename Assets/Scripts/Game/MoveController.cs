using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour 
{
    Snake snake;
    SoundController soundController;

    private void Start()
    {
        soundController = Object.FindObjectOfType<SoundController>();
    }

    public void MoveUp()
    {
        snake = Object.FindObjectOfType<Snake>();

        if (snake.canIt)
        {
            if (snake.gridMoveDirection == Snake.Direction.Left || snake.gridMoveDirection == Snake.Direction.Right ||
           (snake.gridMoveDirection == Snake.Direction.Stop && snake.oldGridMoveDirection != Snake.Direction.Down))
            {
                snake.canIt = false;
                soundController.ClickSoundPlay();
                Time.timeScale = 1;
                if (snake.dizziness)
                {
                    snake.gridMoveDirection = Snake.Direction.Down;
                    snake.oldGridMoveDirection = Snake.Direction.Down;
                }
                else
                {
                    snake.gridMoveDirection = Snake.Direction.Up;
                    snake.oldGridMoveDirection = Snake.Direction.Up;
                }
            }
        }
        else
        {
            Handheld.Vibrate();
        }
        
    }

    public void MoveDown()
    {
        snake = Object.FindObjectOfType<Snake>();

        if (snake.canIt)
        {
            if (snake.gridMoveDirection == Snake.Direction.Left || snake.gridMoveDirection == Snake.Direction.Right ||
           (snake.gridMoveDirection == Snake.Direction.Stop && snake.oldGridMoveDirection != Snake.Direction.Up))
            {
                snake.canIt = false;
                soundController.ClickSoundPlay();
                Time.timeScale = 1;
                if (snake.dizziness)
                {
                    snake.gridMoveDirection = Snake.Direction.Up;
                    snake.oldGridMoveDirection = Snake.Direction.Up;
                }
                else
                {
                    snake.gridMoveDirection = Snake.Direction.Down;
                    snake.oldGridMoveDirection = Snake.Direction.Down;
                }
            }
        }
        else
        {
            Handheld.Vibrate();
        }
        
    }

    public void MoveLeft()
    {
        snake = Object.FindObjectOfType<Snake>();

        if (snake.canIt)
        {
            if (snake.gridMoveDirection == Snake.Direction.Up || snake.gridMoveDirection == Snake.Direction.Down ||
           (snake.gridMoveDirection == Snake.Direction.Stop && snake.oldGridMoveDirection != Snake.Direction.Right))
            {
                snake.canIt = false;
                soundController.ClickSoundPlay();
                Time.timeScale = 1;
                if (snake.dizziness)
                {
                    snake.gridMoveDirection = Snake.Direction.Right;
                    snake.oldGridMoveDirection = Snake.Direction.Right;
                }
                else
                {
                    snake.gridMoveDirection = Snake.Direction.Left;
                    snake.oldGridMoveDirection = Snake.Direction.Left;
                }
            }
        }
        else
        {
            Handheld.Vibrate();
        }
        
    }

    public void MoveRight()
    {
        snake = Object.FindObjectOfType<Snake>();

        if (snake.canIt)
        {
            if (snake.gridMoveDirection == Snake.Direction.Up || snake.gridMoveDirection == Snake.Direction.Down ||
           (snake.gridMoveDirection == Snake.Direction.Stop && snake.oldGridMoveDirection != Snake.Direction.Left))
            {
                snake.canIt = false;
                soundController.ClickSoundPlay();
                Time.timeScale = 1;
                if (snake.dizziness)
                {
                    snake.gridMoveDirection = Snake.Direction.Left;
                    snake.oldGridMoveDirection = Snake.Direction.Left;
                }
                else
                {
                    snake.gridMoveDirection = Snake.Direction.Right;
                    snake.oldGridMoveDirection = Snake.Direction.Right;
                }
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }
}
