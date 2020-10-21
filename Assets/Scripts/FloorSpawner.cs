using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public static int speed = 10;

    private float _floorTileSize;
    private float _asteroidTileSize;
    private float _viewDistance;

    private int _spawnFloorZCoord;

    private GameObject _floorObject;
    private GameObject _asteroidObject;
    private GameObject _lastFloorElement;
    private GameObject _lastAsteroidElement;


    private void Start()
    {
        _floorObject = (GameObject) Resources.Load($"Prefabs\\FloorElement");
        _asteroidObject = (GameObject) Resources.Load($"Prefabs\\Asteroid");

        _viewDistance = Camera.main.farClipPlane;

        _floorTileSize = _floorObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
        _asteroidTileSize = _asteroidObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;

        _spawnFloorZCoord = FirstFloorGeneration();
    }

    private void Update()
    {
        SpawnFloor();
        SpawnAsteroids();

        #region Speed 2x

        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 2;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            speed /= 2;
        }

        #endregion
    }

    private int FirstFloorGeneration()
    {
        int i = 0;
        while (i <= Mathf.RoundToInt(_viewDistance / _floorTileSize))
        {
            _lastFloorElement = Instantiate(_floorObject, 
                new Vector3(0, 0, i * _floorTileSize),
                Quaternion.Euler(0, 0, 0));
            i++;
        }
        // последний будем рисовать за пределами видимости камеры, его и вернём
       

        _lastAsteroidElement = Instantiate(_asteroidObject,
            new Vector3(0, 0, 20),
            Quaternion.Euler(0, 0, 0));

        return i;
    }

    private void SpawnFloor()
    {
        // вставляем новую плитку, если последняя сдвинулась на размер себя
        if ((_spawnFloorZCoord * _floorTileSize) - _lastFloorElement.transform.position.z >= _floorTileSize)
        {
            _lastFloorElement = Instantiate(_floorObject,
                new Vector3(0, 0, _lastFloorElement.transform.position.z + _floorTileSize), 
                Quaternion.Euler(0, 0, 0));
        }
    }

    private void SpawnAsteroids()
    {
        if ((_spawnFloorZCoord * _asteroidTileSize) - _lastAsteroidElement.transform.position.z >= _asteroidTileSize)
        {
            _lastAsteroidElement = Instantiate(_asteroidObject, 
                new Vector3(0, 0, _lastAsteroidElement.transform.position.z + _asteroidTileSize), 
                Quaternion.Euler(0, 0, 0));
        }
    }
}
