using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float _floorTileHeigh;
    private float _floorTileWidth;
    private float _asteroidTileSize;

    private float _viewDistance;

    private int _spawnFloorZCoord;
    private int _spawnAsteroidZCoord;

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
        _asteroidTileSize = _asteroidObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;

        _spawnFloorZCoord = FirstFloorGeneration();
        _spawnAsteroidZCoord = FirstSpawnAsteroids();
    }

    private void Update()
    {
        SpawnFloor();
        SpawnAsteroids();
    }

    private int FirstFloorGeneration()
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
        // последний будем рисовать за пределами видимости камеры, его и вернём
        
        return i;
    }

    private void SpawnFloor()
    {
        // вставляем новую плитку, если последняя сдвинулась на размер себя
        if ((_spawnFloorZCoord * _floorTileHeigh) - _lastFloorElement.transform.position.z >= _floorTileHeigh)
        {
            _lastFloorElement = Instantiate(_floorObject,
                new Vector3(0, 0, _lastFloorElement.transform.position.z + _floorTileHeigh), 
                Quaternion.Euler(0, 0, 0));
        }
    }
    private int FirstSpawnAsteroids()
    {
        int i = 1;
        float range = _floorTileWidth / 2 - _asteroidTileSize / 2;
        while (i <= Mathf.RoundToInt(_viewDistance / _floorTileHeigh / 2))
        {
            _lastAsteroidElement = Instantiate(_asteroidObject,
                new Vector3(Random.Range(-range, range), 1, (_floorTileHeigh * i * 2) + _asteroidTileSize),
                Quaternion.Euler(0, 0, 0));
            i++;
        }
        return i;
    }

    private void SpawnAsteroids()
    {
        float range = _floorTileWidth / 2 - _asteroidTileSize / 2;
        if ((_spawnAsteroidZCoord * _asteroidTileSize) - _lastAsteroidElement.transform.position.z >= _asteroidTileSize)
        {
            _lastAsteroidElement = Instantiate(_asteroidObject,
                new Vector3(Random.Range(-range, range), 1, _lastAsteroidElement.transform.position.z + _asteroidTileSize),
                Quaternion.Euler(0, 0, 0));
        }
    }
}
