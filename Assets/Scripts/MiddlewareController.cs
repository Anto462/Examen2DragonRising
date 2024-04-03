using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

public class MiddlewareController : MonoBehaviour
{
    public UnityEvent OnAttack;

    public void OnMeleeAttack()
    {
        OnAttack?.Invoke();
    }
}
