using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    // количество астероидов на одну плитку при старте
    private float _asteroidCount = 2f;
    private float _asteroidTimeSpawn;
    private float _floorTileHeigh;
    private float _floorTileWidth;
    private float _asteroidTileSize;

    private float _viewDistance;
    
    private GameObject _floorObject;
    private GameObject _lastFloorElement;
    private GameObject _asteroidObject;
    private GameObject _lastAsteroidElement;

    private void Start()
    {
        _viewDistance = Camera.main.farClipPlane;

        _floorObject = (GameObject) Resources.Load($"Prefabs\\FloorElement");
        _asteroidObject = (GameObject)Resources.Load($"Prefabs\\Asteroid");

        _floorTileHeigh = _floorObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
        _floorTileWidth = _floorObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        _asteroidTileSize = _asteroidObject.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.z;

        GameManager.OnGameOver += GameOver;

        // чуть чаще первоначальной генерации
        _asteroidTimeSpawn = 0.3f * _asteroidCount;

        FirstFloorGeneration();
        FirstSpawnAsteroids();

        StartCoroutine(SpawnAsteroids());
    }

    void OnDestroy()
    {
        GameManager.OnGameOver -= GameOver;
    }

    private void Update()
    {
        SpawnFloor();
    }

    private void FirstFloorGeneration()
    {
        // одну плитку позади ракеты
        int i = -1;
        while (i <= Mathf.RoundToInt(_viewDistance / _floorTileHeigh))
        {
            _lastFloorElement = Instantiate(_floorObject, 
                new Vector3(0, 0, i * _floorTileHeigh),
                Quaternion.Euler(0, 0, 0));
            i++;
        }
    }

    private void SpawnFloor()
    {
        // вставляем новую плитку, если последняя сдвинулась на размер
        if (_viewDistance - _lastFloorElement.transform.position.z >= _floorTileHeigh)
        {
            _lastFloorElement = Instantiate(_floorObject,
                new Vector3(0, 0, _lastFloorElement.transform.position.z + _floorTileHeigh), 
                Quaternion.Euler(0, 0, 0));
        }
    }
    private void FirstSpawnAsteroids()
    {
        int i = 1;
        float range = _floorTileWidth / 2 - _asteroidTileSize / _asteroidCount;
        while (i <= Mathf.RoundToInt(_viewDistance / _floorTileHeigh / 2))
        {
            _lastAsteroidElement = Instantiate(_asteroidObject,
                new Vector3(Random.Range(-range, range), 1, (_floorTileHeigh * i * _asteroidCount) + _asteroidTileSize),
                Quaternion.Euler(0, 0, 0));
            i++;
        }
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(_asteroidTimeSpawn * gameManager.gameComplexity);

            float range = _floorTileWidth / 2 - _asteroidTileSize / 2;
            _lastAsteroidElement = Instantiate(_asteroidObject,
                new Vector3(Random.Range(-range, range), 1, _viewDistance + _asteroidTileSize),
                Quaternion.Euler(0, 0, 0));
        }
    }

    private void GameOver()
    {
        StopAllCoroutines();
    }
}
