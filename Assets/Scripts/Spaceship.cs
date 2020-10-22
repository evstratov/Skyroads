using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Spaceship : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GameManager _gameManager;

    // направление движения корабля
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = (_moveDirection * _gameManager.moveSpeed);
    }

    private void Update()
    {
        #region Moving
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 newDirection = new Vector3(-1, 0, 0);

            if (CanMove(newDirection))
            {
                _moveDirection = newDirection;
                StartCoroutine(LeftIncline());
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 newDirection = new Vector3(1, 0, 0);

            if (CanMove(newDirection))
            {
                StartCoroutine(RightIncline());
                _moveDirection = newDirection;
            }
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || !CanMove(_moveDirection))
        {
            StartCoroutine(IdleState());
            _moveDirection = Vector3.zero;
        }
        #endregion
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3 newPos = transform.position + direction;
        RaycastHit hit;
        Ray ray = new Ray(newPos, Vector3.down);
        Physics.Raycast(ray, out hit);

        if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
        {
            return true;
        }
        else
            return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Asteroid"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _gameManager.GameOver();
        Destroy(gameObject);
    }

    private IEnumerator RightIncline()
    {
        Quaternion angleRotation = Quaternion.Euler(0, 0, -20);
        float t = 0;
        while (t <= 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, angleRotation, t);
            t += 0.1f;
            yield return null;
        }
    }
    private IEnumerator LeftIncline()
    {
        Quaternion angleRotation = Quaternion.Euler(0, 0, 20);
        float t = 0;
        while (t <= 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, angleRotation, t);
            t += 0.1f;
            yield return null;
        }
    }

    private IEnumerator IdleState()
    {
        Quaternion angleRotation = Quaternion.Euler(0, 0, 0);
        float t = 0;
        while(t <= 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, angleRotation, t);
            t += 0.1f;
            yield return null;
        }
    }
}
