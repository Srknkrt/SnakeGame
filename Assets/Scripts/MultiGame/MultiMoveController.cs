using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMoveController : Photon.MonoBehaviour
{
    Movement multi; 
    string playerName;

    private void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        Debug.Log("****** playeer name" + playerName);
    }

    public void MoveUp()
    {
        multi = Object.FindObjectOfType<Movement>();

        if (multi.canIt && playerName == PhotonNetwork.playerName)
        {
            if (multi.gridMoveDirection == MultiGameController.Direction.Left || multi.gridMoveDirection == MultiGameController.Direction.Right ||
           (multi.gridMoveDirection == MultiGameController.Direction.Stop && multi.oldGridMoveDirection != MultiGameController.Direction.Down))
            {
                multi.canIt = false;
                multi.gridMoveDirection = MultiGameController.Direction.Up;
                multi.oldGridMoveDirection = MultiGameController.Direction.Up;
            }
        }
        else
        {
            Handheld.Vibrate();
        }

    }

    public void MoveDown()
    {
        multi = Object.FindObjectOfType<Movement>();

        if (multi.canIt && playerName == PhotonNetwork.playerName)
        {
            if (multi.gridMoveDirection == MultiGameController.Direction.Left || multi.gridMoveDirection == MultiGameController.Direction.Right ||
           (multi.gridMoveDirection == MultiGameController.Direction.Stop && multi.oldGridMoveDirection != MultiGameController.Direction.Up))
            {
                multi.canIt = false;
                multi.gridMoveDirection = MultiGameController.Direction.Down;
                multi.oldGridMoveDirection = MultiGameController.Direction.Down;
            }
        }
        else
        {
            Handheld.Vibrate();
        }

    }

    public void MoveLeft()
    {
        multi = Object.FindObjectOfType<Movement>();

        if (multi.canIt && playerName == PhotonNetwork.playerName)
        {
            if (multi.gridMoveDirection == MultiGameController.Direction.Up || multi.gridMoveDirection == MultiGameController.Direction.Down ||
           (multi.gridMoveDirection == MultiGameController.Direction.Stop && multi.oldGridMoveDirection != MultiGameController.Direction.Right))
            {
                multi.canIt = false;
                multi.gridMoveDirection = MultiGameController.Direction.Left;
                multi.oldGridMoveDirection = MultiGameController.Direction.Left;
            }
        }
        else
        {
            Handheld.Vibrate();
        }

    }

    public void MoveRight()
    {
        multi = Object.FindObjectOfType<Movement>();

        if (multi.canIt && playerName == PhotonNetwork.playerName)
        {
            if (multi.gridMoveDirection == MultiGameController.Direction.Up || multi.gridMoveDirection == MultiGameController.Direction.Down ||
           (multi.gridMoveDirection == MultiGameController.Direction.Stop && multi.oldGridMoveDirection != MultiGameController.Direction.Left))
            {
                multi.canIt = false;
                multi.gridMoveDirection = MultiGameController.Direction.Right;
                multi.oldGridMoveDirection = MultiGameController.Direction.Right;
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }
}
