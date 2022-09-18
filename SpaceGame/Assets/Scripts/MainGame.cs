using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public LoadTask[] tasks;
    private CameraController _cameraController;
    private GameState _gameState;
    private LoadTask _currentTask;
    private float asteroidTimer;
    [SerializeField] private float MaxAsteroidTimer = 180;
    [SerializeField] private float MinAsteroidTimer = 60;
    [SerializeField] private float timerDifficultyDecrement = 15;
    [SerializeField] private string initialCameraTag = "PlayerCamera";

    public LoadTask CurrentTask
    {
        set { _currentTask = value;}
        get { return _currentTask; }
    }
    
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

    public enum GameState
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
        _playerShip = FindObjectOfType<Ship>();
        //get all the game task
        tasks = FindObjectsOfType<LoadTask>();
        _cameraController = GetComponent<CameraController>();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _cameraController.SwitchToCameraWithTag(initialCameraTag);
        _gameState = GameState.Start;
    }

    void InitialScene()
    {
        //player startup audio
        //have startup text
        asteroidTimer = MaxAsteroidTimer;
    }

    void GameOver()
    {
        // call game over white screen
        Application.Quit();
        
    }

    //check all conditions and update game state naturally
    void Update()
    {
        while (asteroidTimer > 0)
        {
            
        }

        if (_gameState == GameState.GameOver)
        {
            GameOver();
        }

    }
    
    

    // Update is called once per frame
    void FixedUpdate()
    {
        asteroidTimer -= Time.fixedDeltaTime;
    }
}