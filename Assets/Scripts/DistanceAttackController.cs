using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DistanceAttackController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    Transform target;

    [SerializeField]
    float distanceToTarget;

    [SerializeField]
    float attackRange = 1.0F;

    [SerializeField]
    int attackRate = 1;

    private float _nextAttackTime;

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= distanceToTarget) 
        {
            if (Time.time >= _nextAttackTime) {
                animator.SetTrigger("attack");
                _nextAttackTime = Time.time + attackRange / attackRate;
            }
        }
    }
}
