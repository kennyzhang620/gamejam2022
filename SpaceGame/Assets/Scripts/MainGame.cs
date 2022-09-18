using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public Task[] tasks;
    private CameraController _cameraController;
    private GameState _gameState;
    private Task _currentTask;
    private Player _player;
    private Ship _playerShip;
    private static MainGame _mainGameInstance = null;

    public static MainGame Instance
    {
        get{return _mainGameInstance;}
    }
    
    public CameraController CameraController
    {
        get{return _cameraController;}
    }
    public Ship PlayerShip
    {
        get { return _playerShip; }
    }

    enum GameState
    {
        Start,
        Walking,
        Tasks,
        FlyingShip,
        GameOver
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        _mainGameInstance = this;
        //get all the game task
        tasks = FindObjectsOfType<Task>();
        _cameraController = GetComponent<CameraController>();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _cameraController.SwitchToCameraWithTag("ShipCamera");
        _gameState = GameState.Start;
    }

    void InitialScene()
    {
        //player starts in front of ship 
        //alarms blaring 
        //dark except red emergency lights
        //ship AI says some stuff
        //player must switch on backup power switch 
    }

    //call this function to switch game state 
    void TriggerTask(string tag)
    {
        
        
        
    }

    //check all conditions and update game state naturally
    void UpdateGameState()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameState();
        
        if(_gameState==GameState.Start)
            InitialScene();
        
        //do more
    }
}