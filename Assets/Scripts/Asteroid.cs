using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _tileSize;
    private GameManager _gameManager;
    private GameObject _spaceShip;

    private bool isAdded = false;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _spaceShip = GameObject.FindWithTag("Spaceship");
        _tileSize = GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * _gameManager.moveSpeed * Time.deltaTime);

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
