using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    public int maxHealth;

    [SerializeField]
    GameObject canavsgameover;

    public int currentHealth;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //Morimos
            anim.SetBool("IsDead", true);
            StartCoroutine(Espera());

        }
    }

    IEnumerator Espera()
    {
        yield return new WaitForSeconds(2.0F);
        ActiveCanvas();
    }

    void ActiveCanvas()
    {
        canavsgameover.SetActive(true);
    }
}
