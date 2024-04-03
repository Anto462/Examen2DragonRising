using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : MonoBehaviour
{
    [SerializeField]
    float waitTime;

    [SerializeField]
    float rotationSpeed;

    Rigidbody2D _rigidbody;

    bool isFalling;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isFalling)
        {
            transform.Rotate(new Vector3(0.0F, 0.0F, -rotationSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall(other));
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Fall(Collision2D other)
    {
        yield return new WaitForSeconds(waitTime);
        isFalling = true;

        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), other.collider);
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.AddForce(new Vector2(0.1F, 0.0F));
    }
}
