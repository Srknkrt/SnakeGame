using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private GameObject food;
    [SerializeField] private float hiz;

    List<Transform> Snake = new List<Transform>();

    Controller controller;
    EatManager eatManager;

    int skor;
    int[] areaLimit = {1,22};
    bool gameOver;
    Direction direction = Direction.Stop;

    public enum Direction
    {
        Up,
        Left,
        Down,
        Right,
        Stop
    }

    private void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        eatManager = GameObject.Find("EatManager").GetComponent<EatManager>();
        gameOver = false;
        skor = 0;
        YilanKafasiOlustur();
        YemekOlustur();
        StartCoroutine(HareketEt());
    }

    private void YilanKafasiOlustur()
    {
        transform.position = RastgeleKonumOlustur();
        Snake.Add(transform);
    }

    private Vector2 RastgeleKonumOlustur()
    {
        int x, y;
        do
        {
            x = Random.Range(areaLimit[0], areaLimit[1]);
            y = Random.Range(areaLimit[0], areaLimit[1]);
        } while (KareBosMu(x, y));
        return new Vector2(x, y);
    }

    private bool KareBosMu(int x, int y)
    {
        foreach (Transform transform in Snake)
        {
            if (transform.position == new Vector3(x, y, 0))
            {
                return true;
            }
        }
        return false;
    }

    private void YemekOlustur()
    {
        food.transform.position = RastgeleKonumOlustur();
    }

    private IEnumerator HareketEt()
    {
        while (!gameOver)
        {
            VucuduTakipEt();

            controller.OyunHareketli();

            Vector2 pozisyon = transform.position;
            switch (direction)
            {
                case Direction.Up:
                    pozisyon += Vector2.up;
                    break;
                case Direction.Left:
                    pozisyon += Vector2.left;
                    break;
                case Direction.Down:
                    pozisyon += Vector2.down;
                    break;
                case Direction.Right:
                    pozisyon += Vector2.right;
                    break;
                case Direction.Stop:
                    OyunDur();
                    break;
            }
            transform.position = pozisyon;

            yield return new WaitForSeconds(1 / hiz);
        }
    }

    private void VucuduTakipEt()
    {
        if (direction != Direction.Stop)
        {
            for (int i = Snake.Count - 1; i > 0; i--)
            {
                Snake[i].position = Snake[i - 1].position;
            }
        }
    }

    private void OyunDur()
    {
        controller.OyunDuruk();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && direction != Direction.Down)
        {
            direction = Direction.Up;
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && direction != Direction.Up)
        {
            direction = Direction.Down;
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && direction != Direction.Right)
        {
            direction = Direction.Left;
        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && direction != Direction.Left)
        {
            direction = Direction.Right;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            direction = Direction.Stop;
        }
        if (gameOver && Input.GetKeyDown(KeyCode.Return))
        {
            Restart();
        }

    }

    private void Restart()
    {
        SceneManager.LoadScene("Snake");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Eat();
        }
        if (collision.CompareTag("Wall") || (collision.CompareTag("SnakeBody") && Snake.Count > 2))
        {
            Dead();
        }
    }

    private void Eat()
    {
        GameObject snakeBody = Instantiate(snakeBodyPrefab);
        Snake.Add(snakeBody.transform);
        Snake[Snake.Count - 1].position = Snake[Snake.Count - 2].position;
        skor += 5;
        controller.SkorGuncelle(skor);
        eatManager.YemekYendi();
        YemekOlustur();
    }

    private bool IlkYemMi()
    {
        bool ilk = (Snake.Count == 1) ? true : false;
        return ilk;
    }

    private void Dead()
    {
        gameOver = true;
        controller.OyunBitti();
    }
}
