using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorElement : MonoBehaviour
{
    private float _tileSize;

    private void Start()
    {
        _tileSize = GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * FloorSpawner.speed * Time.deltaTime);

        if (transform.position.z + _tileSize < Camera.main.transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}
