using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] private GameObject AsteroidPrefab;
    [SerializeField] private int MaxAsteroids = 50;
    [SerializeField] private int MinAsteroids = 20;
    [SerializeField] private int MaxAsteroidScale = 6;
    [SerializeField] private int MinAsteroidScale = 2;
    [SerializeField] private int MaxAsteroidSpawnDistance = 150;
    [SerializeField] private int MinAsteroidSpawnDistance = 40;
    [SerializeField] private float DistanceReset = 500;

    private List<Asteroid> _asteroids;

    void SpawnAsteroidsAroundShip()
    {
        Vector3 shipPos = MainGame.Instance.PlayerShip.transform.position;
        int size = Random.Range(MinAsteroids, MaxAsteroids);
        _asteroids = new List<Asteroid>();
        for (int spawnCounter = 0; spawnCounter < size; spawnCounter++)
        {
            Vector3 randPos = Random.onUnitSphere;
            randPos +=  shipPos - randPos;
            randPos.x += Random.Range(MinAsteroidSpawnDistance, MaxAsteroidSpawnDistance);
            randPos.y += Random.Range(MinAsteroidSpawnDistance, MaxAsteroidSpawnDistance);
            randPos.z += Random.Range(MinAsteroidSpawnDistance, MaxAsteroidSpawnDistance);
            // Vector3 randPos = shipPos;
            // int sign = Random.Range(-1, 1);
            // if (sign <= 0)
            //     sign = -1;
            // else
            //     sign = 1;
            //
            // randPos.x = sign * Random.Range(MinAsteroidSpawnDistance, MaxAsteroidSpawnDistance);
            // randPos.z -= sign * Random.Range(MinAsteroidSpawnDistance, MaxAsteroidSpawnDistance);
            // randPos.y = sign * Random.Range(MinAsteroidSpawnDistance, MaxAsteroidSpawnDistance);
            
            GameObject asteroid = Instantiate(AsteroidPrefab, randPos, Quaternion.identity);
            float scale = Random.Range(MinAsteroidScale, MaxAsteroidScale);
            asteroid.transform.localScale = Vector3.one * scale;
            var _astroid = asteroid.GetComponent<Asteroid>();
            var randInt = Random.Range(1, 4);
            if (randInt == 1)
                _astroid.State = Asteroid.AsteroidState.Idle;
            else if (randInt == 2)
                _astroid.State = Asteroid.AsteroidState.IdleMove;
            else if (randInt == 3)
            {
                _astroid.SetTarget(MainGame.Instance.PlayerShip.gameObject);
                _astroid._isTargetShip = true;
                _astroid.State = Asteroid.AsteroidState.TargetLocked;
            }

            _asteroids.Add(_astroid);

        }

    }

    void CleanupDestroyedAsteroids()
    {
        var temp = new List<Asteroid>();
        foreach (var astr in _asteroids)
        {
            if (astr.State == Asteroid.AsteroidState.Destroyed)
                DestroyImmediate(astr.gameObject);
            else
                temp.Add(astr);
        }

        _asteroids = temp;
    }

    void CleanupAllAsteroids()
    {
        foreach (var astr in _asteroids)
            DestroyImmediate(astr.gameObject);
        _asteroids = new List<Asteroid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SpawnAsteroidsAroundShip();
    }

    private void Update()
    {
        CleanupDestroyedAsteroids();
        var closest = FindClosestAsteroid(MainGame.Instance.PlayerShip.transform.position);
        if (closest == null ||
            (closest.transform.position - MainGame.Instance.PlayerShip.transform.position).magnitude
            >= DistanceReset)
        {
            CleanupAllAsteroids();
            SpawnAsteroidsAroundShip();
        }

    }

    Asteroid FindClosestAsteroid(Vector3 position)
    {
        if (_asteroids.Count > 0)
        {
            _asteroids.Sort(delegate(Asteroid a1, Asteroid a2)
            {
                return (int)((position - a1.transform.position).magnitude -
                             (position - a2.transform.position).magnitude);
            });
            
            return _asteroids[0];
        }

        return null;
    }

}
