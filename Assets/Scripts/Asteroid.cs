using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _tileSize;
    private GameManager _gameManager;
    private GameObject _spaceShip;
    private Transform _rotateAsteroidObj;

    private bool isAdded = false;

    private Vector3 rotationVector;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _spaceShip = GameObject.FindWithTag("Spaceship");
        _tileSize = GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.z;

        _rotateAsteroidObj = GetComponentsInChildren<Transform>()[1];
        rotationVector = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * _gameManager.GetSpeed * Time.deltaTime);

        _rotateAsteroidObj.Rotate(rotationVector);

        if (transform.position.z + _tileSize < Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }

        if (!isAdded && _spaceShip != null && _spaceShip.transform.position.z > transform.position.z)
        {
            isAdded = true;
            _gameManager.AddAsteroid();
        }
    }
}
