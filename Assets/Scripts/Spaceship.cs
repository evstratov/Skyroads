using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Spaceship : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;

    // направление движения корабля
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = (_moveDirection * FloorSpawner.speed);
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
                _animator.SetBool("Left", true);
                _animator.SetBool("Right", false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 newDirection = new Vector3(1, 0, 0);

            if (CanMove(newDirection))
            {
                _animator.SetBool("Right", true);
                _animator.SetBool("Left", false);
                _moveDirection = newDirection;
            }
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || !CanMove(_moveDirection))
        {
            _moveDirection = Vector3.zero;
            _animator.SetBool("Left", false);
           _animator.SetBool("Right", false);
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
        FloorSpawner.speed = 0;
        Destroy(gameObject);
    }
}
