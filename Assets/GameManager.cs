using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lives;
    private int score;
    private void Start()
    {
        NewGame();
    }
    public void NewGame()
    {
        lives = 3;
        score = 0;
    }

    public void LevelComplete()
    {
        score += 100;
    }
    public void LevelFail()
    {
        lives--;
        if (lives ==0)
        {
            NewGame();
        }
        else
        {

        }
    }
}
