using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Photon.MonoBehaviour
{
    MultiGameController controller;

    float speed;
    float score;
    public bool canIt;
    Vector3 gridPosition;
    int width = 20;
    int height = 20;
    public MultiGameController.Direction gridMoveDirection;
    public MultiGameController.Direction oldGridMoveDirection;

    void Start()
    {
        controller = Object.FindObjectOfType<MultiGameController>();


        

        canIt = true;
        speed = 9f;
        score = 0f;

        gridMoveDirection = MultiGameController.Direction.Stop;
        oldGridMoveDirection = MultiGameController.Direction.Up;
        gridPosition = new Vector3(transform.position.x, transform.position.y);
        StartCoroutine(HandleGridMovement());
    }

    IEnumerator HandleGridMovement()
    {
        while (controller.state == MultiGameController.State.Alive)
        {
            yield return new WaitForSeconds(1 / (speed));
            Vector3 gridMoveDirectionVector = new Vector3(gridPosition.x, gridPosition.y);
            switch (gridMoveDirection)
            {
                case MultiGameController.Direction.Up:
                    gridMoveDirectionVector = new Vector3(0, 1);
                    break;
                case MultiGameController.Direction.Down:
                    gridMoveDirectionVector = new Vector3(0, -1);
                    break;
                case MultiGameController.Direction.Left:
                    gridMoveDirectionVector = new Vector3(-1, 0);
                    break;
                case MultiGameController.Direction.Right:
                    gridMoveDirectionVector = new Vector3(1, 0);
                    break;
                case MultiGameController.Direction.Stop:
                    gridMoveDirectionVector = new Vector3(0, 0);
                    break;
            }
            gridPosition += gridMoveDirectionVector;
            canIt = true;

            gridPosition = ValidateGridPosition(gridPosition);

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }

    float GetAngleFromVector(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        if (dir == new Vector3(0, 0))
            n = 90;
        return n;
    }

    Vector3 ValidateGridPosition(Vector3 gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}
