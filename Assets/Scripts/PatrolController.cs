using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    bool isFacingRight;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    int damage;

    [SerializeField]
    float health;

    Rigidbody2D _rigidbody;

    MiddlewareController _middlewareController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _middlewareController = GetComponentInChildren<MiddlewareController>();
        _middlewareController.OnAttack.AddListener(OnMeleeAttack);
    }

    private void FixedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.40F, groundMask);
        if (!raycastHit)
        {
            FlipX();
        }

        _rigidbody.velocity = new Vector2(speed * Time.fixedDeltaTime, _rigidbody.velocity.y);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FlipX()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0F, 180.0F, 0.0F);
        speed *= -1;
    }

    public void OnMeleeAttack()
    {
        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, 0.3F);
        if (collider != null && collider.CompareTag("Player"))
        {
            HealthController player = collider.GetComponent<HealthController>();
            player.TakeDamage(damage);
        }
    }

    public void EnemyHit(float _damageDone)
    {
        health -= _damageDone;
    }
}
