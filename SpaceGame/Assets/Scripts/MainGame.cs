using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public Task[] tasks;

    enum GameState
    {
        start,
        tasks,
        dodgeAsteroids,
        gameover
    }
    // Start is called before the first frame update
    void Awake()
    {
        //get all the game task
        tasks = FindObjectsOfType<Task>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
