using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Snake : MonoBehaviour
{
    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;
    // Keep Track of Tail
    List<Transform> tail = new List<Transform>();
    // Did the snake eat something?
    bool ate = false;

    // Tail Prefab
    public GameObject tailPrefab;
    // score text
    public Text text;
    public  int score = 0;
    // game over 
    public bool dead = false;
    public GameObject GameOverText ;
    //restart game
    public GameObject RestartText;
    void Start()
    {
        InvokeRepeating("Move", 0.3f, 0.3f);

    }

    void Update()
    {
        if (!dead)
        {
            // Move in a new Direction?
            if (Input.GetKey(KeyCode.RightArrow))
                dir = Vector2.right;
            else if (Input.GetKey(KeyCode.DownArrow))
                dir = -Vector2.up;    // '-up' means 'down'
            else if (Input.GetKey(KeyCode.LeftArrow))
                dir = -Vector2.right; // '-right' means 'left'
            else if (Input.GetKey(KeyCode.UpArrow))
                dir = Vector2.up;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("level"); //Load scene called Game
            }
        }

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("food"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the Food
            Destroy(coll.gameObject);
            score++;
            text.text = " Score: " + score;
        }
        // Collided with Tail or Border
        else
        {
            dir = Vector2.zero;
            dead = true;
            GameOverText.SetActive(true);
            RestartText.SetActive(true);

            // ToDo 'You lose' screen
        }
    }
    void Move()
    {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);
        // Ate something? Then insert new Element into gap
        if (ate)
        {
            // Load Prefab into the world
            GameObject g = (GameObject)Instantiate(tailPrefab,v,Quaternion.identity);
            // Keep track of it in our tail list
            tail.Insert(0, g.transform);
            // Reset the flag
            ate = false;
        }

        // Do we have a Tail?
        if (tail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);

        }
    }

    }
