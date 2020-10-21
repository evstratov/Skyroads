using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorElement : MonoBehaviour
{
    private float _tileSize;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _tileSize = GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * _gameManager.moveSpeed * Time.deltaTime);

        if (transform.position.z + _tileSize < Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}
