using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameController : MonoBehaviour
{
    [SerializeField] private GameObject SnakeHead;
    [SerializeField] private List<GameObject> SnakeBodies;
    [SerializeField] private GameObject SnakeTail;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Stop
    }

    public enum State
    {
        Alive,
        Dead
    }

    List<Vector3> snakePositions;

    public State state;


    private void Awake()
    {
        float posX = (float)PhotonNetwork.countOfPlayersInRooms;

        PhotonNetwork.Instantiate("SnakeHead", new Vector3(posX * 2, 10, 0), Quaternion.identity, 0);
    }
    private void Start()
    {
        state = State.Alive;

    }

    

   

}
